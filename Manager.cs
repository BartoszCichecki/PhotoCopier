using System.Diagnostics;

namespace PhotoCopier;

internal struct Photo
{
    internal required string Path { get; init; }
    internal required string[] CompanionPaths { get; init; }
}

internal struct CopyResult
{
    internal required int CopiedPhotos { get; init; }
    internal required int FailedHashChecks { get; init; }
}

internal static class Manager
{
    internal static string[] GetDrives()
    {
        return DriveInfo.GetDrives()
            .Where(di => di.DriveType == DriveType.Removable)
            .Where(di => Directory.Exists(Path.Combine(di.Name, "DCIM")))
            .Select(di => di.Name)
            .ToArray();
    }

    internal static Task<List<Photo>> GetPhotosAsync(string drive, string extension) => Task.Run(() =>
    {
        var paths = new List<Photo>();
        GetPhotos(paths, Path.Combine(drive, "DCIM"), extension);
        return paths;
    });

    private static void GetPhotos(List<Photo> paths, string path, string extension)
    {
        if (!Directory.Exists(path))
            return;

        foreach (var subDirectory in Directory.GetDirectories(path))
            GetPhotos(paths, subDirectory, extension);

        var files = Directory.GetFiles(path);

        foreach (var file in files)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
            var fileExtension = Path.GetExtension(file);

            if (!fileExtension.Equals(extension, StringComparison.InvariantCultureIgnoreCase))
                continue;

            var companionFiles = files
                .Where(f => Path.GetFileNameWithoutExtension(f).Equals(fileNameWithoutExtension, StringComparison.InvariantCultureIgnoreCase))
                .Where(f => !Path.GetExtension(f).Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase))
                .ToArray();

            paths.Add(new Photo { Path = file, CompanionPaths = companionFiles });
        }
    }

    internal static Task<List<Photo>> FilterPhotosAsync(List<Photo> photos, string destinationDirectory, string folderName, bool overwrite) => Task.Run(() =>
    {
        if (overwrite)
            return photos;

        var filtered = new List<Photo>();

        foreach (var photo in photos)
        {
            var fileName = Path.GetFileName(photo.Path);
            var subDirectory = File.GetCreationTime(photo.Path).ToString(folderName);
            var destinationPath = Path.Combine(destinationDirectory, subDirectory, fileName);

            if (File.Exists(destinationPath))
                continue;

            filtered.Add(photo);
        }

        return filtered;
    });

    internal static async Task<CopyResult> CopyAsync(List<Photo> photos,
        string destinationDirectory,
        string folderName,
        bool copyCompanionFiles,
        bool verify,
        Action<int, int> updateProgress,
        CancellationToken cancellationToken)
    {
        var copiedPhotos = 0;
        var failedHashChecks = 0;

        for (var i = 0; i < photos.Count; i++)
        {
            var photo = photos[i];

            var subDirectory = File.GetCreationTime(photo.Path).ToString(folderName);
            var fileName = Path.GetFileName(photo.Path);

            var destinationSubDirectory = Path.Combine(destinationDirectory, subDirectory);
            if (!Directory.Exists(destinationSubDirectory))
                Directory.CreateDirectory(destinationSubDirectory);

            var destinationPath = Path.Combine(destinationSubDirectory, fileName);

            await FileUtils.CopyAsync(photo.Path, destinationPath, cancellationToken).ConfigureAwait(false);

            if (verify)
            {
                var hashMatch = await FileUtils.CompareHash(photo.Path, destinationPath, cancellationToken).ConfigureAwait(false);
                if (!hashMatch)
                    failedHashChecks++;
            }

            Debug.WriteLine($"Copied {photo.Path} to {destinationPath}");

            if (copyCompanionFiles)
            {
                foreach (var companionFile in photo.CompanionPaths)
                {
                    var companionFileName = Path.GetFileName(companionFile);
                    var companionDestinationPath = Path.Combine(destinationSubDirectory, companionFileName);

                    await FileUtils.CopyAsync(companionFile, companionDestinationPath, cancellationToken)
                        .ConfigureAwait(false);

                    Debug.WriteLine($"Copied {companionFile} to {companionDestinationPath}");
                }
            }

            copiedPhotos++;

            updateProgress(i, photos.Count);
        }

        return new CopyResult { CopiedPhotos = copiedPhotos, FailedHashChecks = failedHashChecks };
    }
}
