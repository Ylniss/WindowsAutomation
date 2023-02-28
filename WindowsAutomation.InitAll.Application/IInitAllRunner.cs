using WindowsAutomation.InitAll.Application.PackageInstallers;
using WindowsAutomation.Shared.Filesystem.DirCleaner;
using WindowsAutomation.Shared.Filesystem.DirCopier;
using WindowsAutomation.Shared.Filesystem.DirMaker;
using WindowsAutomation.Shared.Git;
using WindowsAutomation.Shared.Os.Windows.CursorChanger;
using WindowsAutomation.Shared.Os.Windows.Pinner;

namespace WindowsAutomation.InitAll.Application;

public interface IInitAllRunner
{
    IEnumerable<IPackageInstaller> PackageInstallers { get; }

    IDirCleaner DirCleaner { get; }
    IDirMaker DirMaker { get; }
    IDirCopier DirCopier { get; }
    IGitClient GitClient { get; }
    ICursorChanger CursorChanger { get; }
    IPinner Pinner { get; }

    InitAllConfig GetConfigFromJson();
    Task InstallPackages();
    void CloneReposFromGitHub(GithubCredentials githubCredentials, string[] repoNames, string repoPath);
    void SwapPowerShellProfileWithSymbolicLink(string pathToTarget);
    void CreateInitialFolderStructure(string[] directories);
    void CopyDirectories(CopyPaths[] copyPaths);
    void CleanDesktopAndRecycleBin();
}