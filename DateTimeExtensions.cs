namespace PhotoCopier;

internal static class DateTimeExtensions
{
    internal static string ToDirectoryName(this DateTime date) => date.ToString("yyyyMMdd");
}
