namespace WindowsAutomation.InitAll.Application;

public record GithubCredentials(string Username, string AccessToken);

public record Paths(string Repo);

public record InitAllConfig(GithubCredentials GithubCredentials, string[] ReposToClone, Paths Paths,
    string[] FolderStructure);