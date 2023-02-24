using System.Reactive.Linq;
using System.Reactive.Subjects;
using WindowsAutomation.Shared.Web.Downloader;

// ReSharper disable InconsistentNaming

namespace WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller.AppInstallers;

public abstract class AppInstaller : IAppInstaller
{
    public abstract string AppName { get; }
    public abstract string SetupPath { get; }

    protected readonly IWebDownloader _webDownloader;

    protected readonly Subject<PackageInstallationStep> _whenInstallStarted = new();
    protected readonly Subject<string> _whenSetupOutputReceived = new();

    public IObservable<PackageInstallationStep> WhenInstallStarted =>
        _whenInstallStarted.AsObservable();

    public IObservable<string> WhenSetupOutputReceived =>
        _whenSetupOutputReceived.AsObservable();

    public IObservable<string> WhenDownloadStarted { get; }
    public IObservable<double?> WhenDownloadProgressReceived { get; }

    public AppInstaller(IWebDownloader webDownloader)
    {
        _webDownloader = webDownloader;
        WhenDownloadStarted = _webDownloader.WhenDownloadStarted;
        WhenDownloadProgressReceived = _webDownloader.WhenDownloadProgressReceived;
    }

    public abstract Task InstallApp();
}