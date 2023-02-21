using WindowsAutomation.InitAll.Application.PackageInstallers;

namespace WindowsAutomation.InitAll.Application;

public interface IInitAllRunner
{
    IEnumerable<IPackageInstaller> PackageInstallers { get; }
    Task RunCoreLogic();
}