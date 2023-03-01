using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller.AppInstallers;

public interface IAppInstaller
{
    string AppName { get; }
    string SetupPath { get; }
    RxEvent<PackageInstallationStep> WhenInstall { get; }
    RxEvent<string> WhenSetupOutputReceive { get; }

    Task InstallApp();
}