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

public record DateTimeFormat
{
    public string ShortDate { get; }
    public string LongDate { get; }
    public string ShortTime { get; }
    public string LongTime { get; }
    public string FirstDayOfWeek { get; }

    public DateTimeFormat(string shortDate, string longDate, string shortTime, string longTime, string firstDayOfWeek)
    {
        ShortDate = shortDate;
        LongDate = longDate;
        ShortTime = shortTime;
        LongTime = longTime;
        FirstDayOfWeek = firstDayOfWeek.ToLower() switch
        {
            "monday" => "0",
            "tuesday" => "1",
            "wednesday" => "2",
            "thursday" => "3",
            "friday" => "4",
            "saturday" => "5",
            "sunday" => "6",
            _ => "0"
        };
    }
}

public record InitAllConfig
{
    public GithubCredentials GithubCredentials { get; }
    public string[] ReposToClone { get; }
    public string[] StartupApps { get; }
    public string[] StartupAppsToRemove { get; }
    public SourceTargetPaths[] ShortcutDirectories { get; }
    public Paths Paths { get; }
    public string[] FolderStructure { get; }
    public SourceTargetPaths[] CopyDirectories { get; }
    public string[] PinToQuickAccess { get; }
    public string TimeZoneId { get; }
    public DateTimeFormat DateTimeFormat { get; }
    public Theme CursorTheme { get; }

    public InitAllConfig(GithubCredentials githubCredentials, string[] reposToClone, Paths paths,
        string[] folderStructure, SourceTargetPaths[] copyDirectories, SourceTargetPaths[] shortcutDirectories,
        string[] pinToQuickAccess, string[] startupApps, string[] startupAppsToRemove,
        string timeZoneId, DateTimeFormat dateTimeFormat, Theme cursorTheme)
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
        StartupAppsToRemove = startupAppsToRemove.Select(directory => directory.AsWindowsPath()).ToArray();

        PinToQuickAccess = pinToQuickAccess.Select(directory => directory.AsWindowsPath()).ToArray();

        TimeZoneId = timeZoneId;
        DateTimeFormat = dateTimeFormat;
        CursorTheme = cursorTheme;
    }
}