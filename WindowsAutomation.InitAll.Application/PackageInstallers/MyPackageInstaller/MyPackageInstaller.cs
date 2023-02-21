using System.Reactive.Linq;
using WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller.AppInstallers;

namespace WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller;

public class MyPackageInstaller : IPackageInstaller
{
    private readonly IEnumerable<IAppInstaller> _appInstallers;

    public IObservable<PackageInstallationStep> WhenInstallStarted { get; }
    public IObservable<string>? WhenDownloadStarted { get; }
    public IObservable<double?>? WhenDownloadProgressReceived { get; }

    public MyPackageInstaller(IEnumerable<AppInstaller> appInstallers)
    {
        var installers = appInstallers.ToList();
        _appInstallers = installers;
        WhenInstallStarted = installers.Select(appInstaller => appInstaller.WhenInstallStarted).Merge();
        WhenDownloadStarted = installers.Select(appInstaller => appInstaller.WhenDownloadStarted).Merge();
        WhenDownloadProgressReceived =
            installers.Select(appInstaller => appInstaller.WhenDownloadProgressReceived).Merge();
    }

    public async Task InstallPackages()
    {
        foreach (var appInstaller in _appInstallers) await appInstaller.InstallApp();
    }
}