namespace WindowsAutomation.InitAll.Application.Installers;

public interface IPackageInstaller
{
    public IObservable<string> WhenInstallStarted { get; }

    public IObservable<string>? WhenDownloadStarted { get; }
    public IObservable<double?>? WhenDownloadProgressReceived { get; }

    Task InstallPackages();
}