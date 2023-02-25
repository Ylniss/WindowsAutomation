using System.Reactive.Linq;
using System.Reactive.Subjects;
using LibGit2Sharp;

namespace WindowsAutomation.Shared.Git;

public class GitClient : IGitClient
{
    public UsernamePasswordCredentials User { get; set; }

    private readonly Subject<(string repo, string destination)> _whenGitCloneStarted = new();
    public IObservable<(string repo, string destination)> WhenGitCloneStarted => _whenGitCloneStarted.AsObservable();

    public void CloneIfNotExists(string gitRepoName, string repo)
    {
        if (User is null) throw new InvalidOperationException($"{nameof(User)} must be initialized");

        var gitRepoPath = $"""{repo}\{gitRepoName}""";
        if (Directory.Exists(gitRepoPath)) return;

        _whenGitCloneStarted.OnNext((gitRepoName, gitRepoPath));
        try
        {
            Repository.Clone($"https://github.com/{User.Username}/{gitRepoName}.git", gitRepoPath,
                new CloneOptions
                {
                    CredentialsProvider = (_, _, _) => User
                });
        }
        catch (Exception ex)
        {
            _whenGitCloneStarted.OnError(ex);
        }
    }
}