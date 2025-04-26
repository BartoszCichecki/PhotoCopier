using System.Diagnostics;

namespace PhotoCopier;

internal struct Photo
{
    internal required string File { get; init; }
    internal required string[] CompanionFiles { get; init; }
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

    internal static Task<List<Photo>> GetPhotosAsync(string drive) => Task.Run(() =>
    {
        var paths = new List<Photo>();
        GetPhotos(paths, Path.Combine(drive, "DCIM"), ".ARW");
        return paths;
    });

    internal static async Task<CopyResult> CopyAsync(List<Photo> photos,
        string destinationDirectory,
        bool verify,
        bool overwrite,
        Action<int, int> updateProgress,
        CancellationToken cancellationToken)
    {
        var copiedPhotos = 0;
        var failedHashChecks = 0;

        for (var i = 0; i < photos.Count; i++)
        {
            var photo = photos[i];

            var subDirectory = File.GetCreationTime(photo.File).ToString("yyyyMMdd");
            var fileName = Path.GetFileName(photo.File);

            var destinationSubDirectory = Path.Combine(destinationDirectory, subDirectory);
            if (!Directory.Exists(destinationSubDirectory))
                Directory.CreateDirectory(destinationSubDirectory);

            var destinationPath = Path.Combine(destinationSubDirectory, fileName);
            if (!overwrite && File.Exists(destinationPath))
                continue;

            await FileUtils.CopyAsync(photo.File, destinationPath, overwrite, cancellationToken).ConfigureAwait(false);

            if (verify)
            {
                var hashMatch = await FileUtils.CompareHash(photo.File, destinationPath, cancellationToken).ConfigureAwait(false);
                if (!hashMatch)
                    failedHashChecks++;
            }

            Debug.WriteLine($"Copied {photo.File} to {destinationPath}");

            foreach (var companionFile in photo.CompanionFiles)
            {
                var companionFileName = Path.GetFileName(companionFile);
                var companionDestinationPath = Path.Combine(destinationSubDirectory, companionFileName);
                if (!overwrite && File.Exists(companionDestinationPath))
                    continue;

                await FileUtils.CopyAsync(companionFile, companionDestinationPath, overwrite, cancellationToken).ConfigureAwait(false);

                Debug.WriteLine($"Copied {companionFile} to {companionDestinationPath}");
            }

            copiedPhotos++;

            updateProgress(i, photos.Count);
        }

        return new CopyResult { CopiedPhotos = copiedPhotos, FailedHashChecks = failedHashChecks };
    }

    private static void GetPhotos(List<Photo> paths, string path, string extension)
    {
        if (!Directory.Exists(path))
            return;

        foreach (var subDirectory in Directory.GetDirectories(path))
            GetPhotos(paths, subDirectory, extension);

        foreach (var file in Directory.GetFiles(path))
        {
            if (!string.Equals(Path.GetExtension(file), extension, StringComparison.InvariantCultureIgnoreCase))
                continue;

            var companionFiles = Directory.GetFiles(path, $"{Path.GetFileNameWithoutExtension(file)}.*", SearchOption.TopDirectoryOnly)
                .Where(f => !string.Equals(Path.GetExtension(f), extension, StringComparison.InvariantCultureIgnoreCase))
                .ToArray();

            paths.Add(new Photo { File = file, CompanionFiles = companionFiles });
        }
    }
}
