using LibGit2Sharp;
using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Git;

public class GitClient : IGitClient
{
    public UsernamePasswordCredentials? User { get; set; }

    public RxEvent<Repo> WhenGitClone { get; } = new();

    public void CloneIfNotExists(string gitRepoName, string repoUri)
    {
        if (User is null) throw new InvalidOperationException($"{nameof(User)} must be initialized");

        var gitRepoPath = $"""{repoUri}\{gitRepoName}""";
        if (Directory.Exists(gitRepoPath)) return;

        Repo repo = new(gitRepoName, gitRepoPath);

        WhenGitClone.Act(repo,
            r =>
            {
                Repository.Clone($"https://github.com/{User.Username}/{r.Name}.git", r.Destination,
                    new CloneOptions
                    {
                        CredentialsProvider = (_, _, _) => User
                    });
            });
    }
}