using LibGit2Sharp;

namespace WindowsAutomation.Shared.Git;

public interface IGitClient
{
    public IObservable<(string repo, string destination)> WhenGitCloneStarted { get; }

    UsernamePasswordCredentials User { get; set; }
    void CloneIfNotExists(string gitRepoName, string destination);
}