using WindowsAutomation.InitAll.Application.PackageInstallers;
using WindowsAutomation.Shared.Filesystem.DirCleaner;
using WindowsAutomation.Shared.Filesystem.DirMaker;
using WindowsAutomation.Shared.Git;

namespace WindowsAutomation.InitAll.Application;

public interface IInitAllRunner
{
    IEnumerable<IPackageInstaller> PackageInstallers { get; }
    public IGitClient GitClient { get; }
    public IDirCleaner DirCleaner { get; }
    public IDirMaker DirMaker { get; }

    Task RunCoreLogic();
}