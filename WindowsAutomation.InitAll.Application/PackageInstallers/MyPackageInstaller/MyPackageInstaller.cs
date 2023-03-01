using System.Reactive.Linq;
using WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller.AppInstallers;
using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller;

public class MyPackageInstaller : IPackageInstaller
{
    private readonly IEnumerable<IAppInstaller> _appInstallers;

    public RxEvent<PackageInstallationStep> WhenInstall { get; } = new();
    public RxEvent<string> WhenSetupOutputReceive { get; } = new();
    public IObservable<string>? WhenDownloadStarted { get; }
    public IObservable<double?>? WhenDownloadProgressReceived { get; }

    public MyPackageInstaller(IEnumerable<AppInstaller> appInstallers)
    {
        var installers = appInstallers.ToList();
        _appInstallers = installers;

        WhenInstall.Merge(installers.Select(appInstaller => appInstaller.WhenInstall));
        WhenSetupOutputReceive.Merge(installers.Select(appInstaller => appInstaller.WhenSetupOutputReceive));

        WhenDownloadStarted = installers.Select(appInstaller => appInstaller.WhenDownloadStarted).Merge();
        WhenDownloadProgressReceived =
            installers.Select(appInstaller => appInstaller.WhenDownloadProgressReceived).Merge();
    }

    public async Task InstallPackages()
    {
        foreach (var appInstaller in _appInstallers) await appInstaller.InstallApp();
    }
}