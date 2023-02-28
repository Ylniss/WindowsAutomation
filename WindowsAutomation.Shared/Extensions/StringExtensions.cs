namespace WindowsAutomation.Shared.Extensions;

public static class StringExtensions
{
    public static string AsWindowsPath(this string path) =>
        path.Replace("~", Constants.CommonPaths.User)
            .Replace("/", "\\");
}