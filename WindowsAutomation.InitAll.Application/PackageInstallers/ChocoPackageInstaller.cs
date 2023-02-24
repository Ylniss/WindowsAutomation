using System.Reactive.Linq;
using System.Reactive.Subjects;
using WindowsAutomation.Shared;
using WindowsAutomation.Shared.Shell;

namespace WindowsAutomation.InitAll.Application.PackageInstallers;

public class ChocoPackageInstaller : IPackageInstaller
{
    private const string ChocoInstallUri = """https://chocolatey.org/install.ps1""";

    private readonly IShellRunner _shellRunner;

    private readonly Subject<PackageInstallationStep> _whenInstallStarted = new();
    public IObservable<PackageInstallationStep> WhenInstallStarted => _whenInstallStarted.AsObservable();
    public IObservable<string>? WhenDownloadStarted { get; }
    public IObservable<double?>? WhenDownloadProgressReceived { get; }
    public IObservable<string> WhenSetupOutputReceived { get; }

    public ChocoPackageInstaller(IShellRunner shellRunner)
    {
        _shellRunner = shellRunner;
        WhenDownloadStarted = _shellRunner.WhenDownloadStarted;
        WhenDownloadProgressReceived = _shellRunner.WhenDownloadProgressReceived;
        WhenSetupOutputReceived = _shellRunner.WhenOutputReceived;
    }

    private async Task InstallChoco()
    {
        await _shellRunner.RunScriptFromWeb(ChocoInstallUri);
    }

    public async Task InstallPackages()
    {
        _whenInstallStarted.OnNext(new PackageInstallationStep(ChocoInstallUri, InstallationStep.RunScript));
        await InstallChoco();

        _whenInstallStarted.OnNext(new PackageInstallationStep(Constants.ChocoPackagesConfig,
            InstallationStep.RunScript));
        await Task.Run(() =>
            _shellRunner.RunScript($"choco install {Constants.ChocoPackagesConfig} -y --acceptlicense --force"));
    }
}