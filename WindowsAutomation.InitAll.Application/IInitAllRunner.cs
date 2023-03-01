using WindowsAutomation.InitAll.Application.PackageInstallers;
using WindowsAutomation.Shared.Filesystem.DirCleaner;
using WindowsAutomation.Shared.Filesystem.DirCopier;
using WindowsAutomation.Shared.Filesystem.DirMaker;
using WindowsAutomation.Shared.Git;
using WindowsAutomation.Shared.Os.Windows.CursorChanger;
using WindowsAutomation.Shared.Os.Windows.Pinner;
using WindowsAutomation.Shared.Os.Windows.StartupAppsAdder;
using WindowsAutomation.Shared.Os.Windows.SystemDateTimeChanger;

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
    IStartupAppsAdder StartupAppsAdder { get; }
    ISystemDateTimeChanger SystemDateTimeChanger { get; }

    InitAllConfig GetConfigFromJson();
    Task InstallPackages();
    void SetupStartupApplications(string[] startupApps);
    void CloneReposFromGitHub(GithubCredentials githubCredentials, string[] repoNames, string repoPath);
    void SwapPowerShellProfileWithSymbolicLink(string pathToTarget);
    void CreateInitialFolderStructure(string[] directories);
    void CopyDirectories(SourceTargetPaths[] copyPaths);
    void PinDirectoriesToQuickAccess(string[] directories);
    void CreateShortcuts(SourceTargetPaths[] paths);
    void CleanDesktopAndRecycleBin();
}