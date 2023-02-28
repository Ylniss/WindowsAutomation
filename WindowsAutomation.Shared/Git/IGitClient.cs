using LibGit2Sharp;
using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Git;

public interface IGitClient
{
    public RxEvent<Repo> WhenGitClone { get; }

    UsernamePasswordCredentials? User { get; set; }
    void CloneIfNotExists(string gitRepoName, string repoUri);
}