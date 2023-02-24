namespace WindowsAutomation.InitAll.Application.PackageInstallers;

public interface IPackageInstaller
{
    IObservable<PackageInstallationStep> WhenInstallStarted { get; }
    IObservable<string>? WhenDownloadStarted { get; }
    IObservable<double?>? WhenDownloadProgressReceived { get; }
    IObservable<string> WhenSetupOutputReceived { get; }

    Task InstallPackages();
}