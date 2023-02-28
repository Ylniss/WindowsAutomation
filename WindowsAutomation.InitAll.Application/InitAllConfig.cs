using WindowsAutomation.Shared;
using WindowsAutomation.Shared.Os.Windows;

namespace WindowsAutomation.InitAll.Application;

public record GithubCredentials(string Username, string AccessToken);

public record Paths
{
    public string Repo { get; }

    public Paths(string repo)
    {
        Repo = repo.Replace("~", Constants.CommonPaths.User);
    }
}

public record CopyPaths
{
    public string From { get; }
    public string To { get; }

    public CopyPaths(string from, string to)
    {
        From = from.Replace("~", Constants.CommonPaths.User);
        To = to.Replace("~", Constants.CommonPaths.User);
    }
}

public record InitAllConfig
{
    public GithubCredentials GithubCredentials { get; }
    public string[] ReposToClone { get; }
    public Paths Paths { get; }
    public string[] FolderStructure { get; }
    public CopyPaths[] CopyDirectories { get; }
    public Theme OsTheme { get; }
    public Theme CursorTheme { get; }

    public InitAllConfig(GithubCredentials githubCredentials, string[] reposToClone, Paths paths,
        string[] folderStructure, CopyPaths[] copyDirectories, Theme osTheme, Theme cursorTheme)
    {
        GithubCredentials = githubCredentials;
        ReposToClone = reposToClone;

        Paths = new Paths(paths.Repo.Replace("~", Constants.CommonPaths.User));
        FolderStructure = folderStructure.Select(directory =>
            directory.Replace("~", Constants.CommonPaths.User)).ToArray();

        CopyDirectories = copyDirectories;
        OsTheme = osTheme;
        CursorTheme = cursorTheme;
    }
}