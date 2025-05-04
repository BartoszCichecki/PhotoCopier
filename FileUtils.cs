using System.Security.Cryptography;

namespace PhotoCopier;

internal static class FileUtils
{
    public static async Task CopyAsync(string sourcePath, string destinationPath, CancellationToken cancellationToken = default)
    {
        try
        {
            await using Stream source = File.Open(sourcePath, FileMode.Open);
            await using Stream destination = File.Create(destinationPath);
            await source.CopyToAsync(destination, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            File.Delete(destinationPath);
            throw;
        }
    }

    public static async Task<bool> CompareHash(string sourcePath, string destinationPath, CancellationToken cancellationToken = default)
    {
        using var sha1 = SHA1.Create();
        var sourceHash = await sha1.ComputeHashAsync(File.Open(sourcePath, FileMode.Open), cancellationToken).ConfigureAwait(false);
        var destinationHash = await sha1.ComputeHashAsync(File.Open(destinationPath, FileMode.Open), cancellationToken).ConfigureAwait(false);
        return sourceHash.SequenceEqual(destinationHash);
    }
}
