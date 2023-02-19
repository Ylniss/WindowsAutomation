using WindowsAutomation.InitAll.Application.Installers;

namespace WindowsAutomation.InitAll.Application;

public interface IInitAllRunner
{
    IEnumerable<IPackageInstaller> PackageInstallers { get; }
    Task RunCoreLogic();
}