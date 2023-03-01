using WindowsAutomation.Shared.Extensions;
using WindowsAutomation.Shared.Os.Windows;

namespace WindowsAutomation.InitAll.Application;

public record GithubCredentials(string Username, string AccessToken);

public record Paths
{
    public string Repo { get; }

    public Paths(string repo)
    {
        Repo = repo.AsWindowsPath();
    }
}

public record SourceTargetPaths
{
    public string From { get; }
    public string To { get; }

    public SourceTargetPaths(string from, string to)
    {
        From = from.AsWindowsPath();
        To = to.AsWindowsPath();
    }
}

public record InitAllConfig
{
    public GithubCredentials GithubCredentials { get; }
    public string[] ReposToClone { get; }
    public string[] StartupApps { get; }
    public SourceTargetPaths[] ShortcutDirectories { get; }
    public Paths Paths { get; }
    public string[] FolderStructure { get; }
    public SourceTargetPaths[] CopyDirectories { get; }
    public string[] PinToQuickAccess { get; }
    public Theme CursorTheme { get; }
    public string TimeZoneId { get; }

    public InitAllConfig(GithubCredentials githubCredentials, string[] reposToClone, Paths paths,
        string[] folderStructure, SourceTargetPaths[] copyDirectories, SourceTargetPaths[] shortcutDirectories,
        string[] pinToQuickAccess, string[] startupApps,
        Theme cursorTheme, string timeZoneId)
    {
        GithubCredentials = githubCredentials;
        ReposToClone = reposToClone;

        Paths = new Paths(paths.Repo);

        FolderStructure = folderStructure.Select(directory => directory.AsWindowsPath()).ToArray();
        CopyDirectories = copyDirectories
            .Select(copyPaths => new SourceTargetPaths(copyPaths.From.AsWindowsPath(), copyPaths.To.AsWindowsPath()))
            .ToArray();
        ShortcutDirectories = shortcutDirectories
            .Select(copyPaths => new SourceTargetPaths(copyPaths.From.AsWindowsPath(), copyPaths.To.AsWindowsPath()))
            .ToArray();
        StartupApps = startupApps.Select(directory => directory.AsWindowsPath()).ToArray();

        PinToQuickAccess = pinToQuickAccess.Select(directory => directory.AsWindowsPath()).ToArray();

        CursorTheme = cursorTheme;
        TimeZoneId = timeZoneId;
    }
}