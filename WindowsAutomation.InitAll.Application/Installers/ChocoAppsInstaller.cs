using System.Reactive.Linq;
using System.Reactive.Subjects;
using WindowsAutomation.Shared;
using WindowsAutomation.Shared.Shell;

namespace WindowsAutomation.InitAll.Application.Installers;

public class ChocoAppsInstaller : IPackageInstaller
{
    private const string ChocoInstallUri = """https://chocolatey.org/install.ps1""";

    private readonly IShellRunner _shellRunner;

    private readonly Subject<string> _whenInstallStarted = new();
    public IObservable<string> WhenInstallStarted => _whenInstallStarted.AsObservable();
    public IObservable<string>? WhenDownloadStarted { get; }
    public IObservable<double?>? WhenDownloadProgressReceived { get; }
    public IObservable<string> WhenChocoScriptOutputReceived { get; }

    public ChocoAppsInstaller(IShellRunner shellRunner)
    {
        _shellRunner = shellRunner;
        WhenDownloadStarted = _shellRunner.WhenDownloadStarted;
        WhenDownloadProgressReceived = _shellRunner.WhenDownloadProgressReceived;
        WhenChocoScriptOutputReceived = _shellRunner.WhenOutputReceived;
    }

    public async Task InstallChoco()
    {
        _whenInstallStarted.OnNext(ChocoInstallUri);
        await _shellRunner.RunScriptFromWeb(ChocoInstallUri);
    }


    public async Task InstallPackages()
    {
        _whenInstallStarted.OnNext(Constants.ChocoPackagesConfig);
        await Task.Run(() =>
            _shellRunner.RunScript($"choco install {Constants.ChocoPackagesConfig} -y --acceptlicense --force"));
    }
}