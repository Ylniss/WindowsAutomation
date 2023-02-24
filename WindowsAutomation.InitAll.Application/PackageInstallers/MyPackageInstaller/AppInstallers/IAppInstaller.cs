namespace WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller.AppInstallers;

public interface IAppInstaller
{
    string AppName { get; }
    string SetupPath { get; }
    IObservable<PackageInstallationStep> WhenInstallStarted { get; }
    IObservable<string> WhenSetupOutputReceived { get; }

    Task InstallApp();
}