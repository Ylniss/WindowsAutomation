using WindowsAutomation.Shared.Rx;
using WindowsAutomation.Shared.Web.Downloader;

namespace WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller.AppInstallers;

public abstract class AppInstaller : IAppInstaller
{
    public abstract string AppName { get; }
    public abstract string SetupPath { get; }

    protected readonly IWebDownloader WebDownloader;

    public RxEvent<PackageInstallationStep> WhenInstall { get; } = new();
    public RxEvent<string> WhenSetupOutputReceive { get; } = new();

    public IObservable<string> WhenDownloadStarted { get; }
    public IObservable<double?> WhenDownloadProgressReceived { get; }

    public AppInstaller(IWebDownloader webDownloader)
    {
        WebDownloader = webDownloader;
        WhenDownloadStarted = WebDownloader.WhenDownloadStarted;
        WhenDownloadProgressReceived = WebDownloader.WhenDownloadProgressReceived;
    }

    public abstract Task InstallApp();
}