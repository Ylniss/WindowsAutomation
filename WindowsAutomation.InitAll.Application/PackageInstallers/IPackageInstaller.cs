using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.InitAll.Application.PackageInstallers;

public interface IPackageInstaller
{
    public RxEvent<PackageInstallationStep> WhenInstall { get; }
    public RxEvent<string> WhenSetupOutputReceive { get; }
    IObservable<string>? WhenDownloadStarted { get; }
    IObservable<double?>? WhenDownloadProgressReceived { get; }

    Task InstallPackages();
}