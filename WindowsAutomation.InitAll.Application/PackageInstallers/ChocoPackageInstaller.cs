using WindowsAutomation.Shared;
using WindowsAutomation.Shared.Rx;
using WindowsAutomation.Shared.Shell;

namespace WindowsAutomation.InitAll.Application.PackageInstallers;

public class ChocoPackageInstaller : IPackageInstaller
{
    private const string ChocoInstallUri = """https://chocolatey.org/install.ps1""";

    private readonly IShellRunner _shellRunner;

    public RxEvent<PackageInstallationStep> WhenInstall { get; } = new();
    public RxEvent<string> WhenSetupOutputReceive { get; }
    public IObservable<string>? WhenDownloadStarted { get; }
    public IObservable<double?>? WhenDownloadProgressReceived { get; }

    public ChocoPackageInstaller(IShellRunner shellRunner)
    {
        _shellRunner = shellRunner;
        WhenDownloadStarted = _shellRunner.WhenDownloadStarted;
        WhenDownloadProgressReceived = _shellRunner.WhenDownloadProgressReceived;
        WhenSetupOutputReceive = _shellRunner.WhenOutputReceive;
    }

    public async Task InstallPackages()
    {
        await WhenInstall.ActAsync(new PackageInstallationStep(ChocoInstallUri, InstallationStep.RunScript),
            async _ =>
            {
                await InstallChoco();

                WhenInstall.StartedSubject.OnNext(new PackageInstallationStep(Constants.ChocoPackagesConfig,
                    InstallationStep.RunScript));
                await Task.Run(() =>
                    _shellRunner.RunScript(
                        $"choco install {Constants.ChocoPackagesConfig} -y --acceptlicense --force"));
            });
    }

    private async Task InstallChoco()
    {
        await _shellRunner.RunScriptFromWeb(ChocoInstallUri);
    }
}