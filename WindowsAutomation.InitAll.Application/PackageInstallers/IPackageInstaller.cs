namespace WindowsAutomation.InitAll.Application.PackageInstallers;

public interface IPackageInstaller
{
    public IObservable<PackageInstallationStep> WhenInstallStarted { get; }

    public IObservable<string>? WhenDownloadStarted { get; }
    public IObservable<double?>? WhenDownloadProgressReceived { get; }

    Task InstallPackages();
}