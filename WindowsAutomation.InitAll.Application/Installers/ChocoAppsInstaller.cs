using WindowsAutomation.Shared;
using WindowsAutomation.Shared.Events;
using WindowsAutomation.Shared.Shell;

namespace WindowsAutomation.InitAll.Application.Installers;

public class ChocoAppsInstaller : IPackageInstaller
{
    private const string ChocoInstallUri = """https://chocolatey.org/install.ps1""";

    private readonly IShellRunner _shellRunner;

    public ChocoAppsInstaller(IShellRunner shellRunner)
    {
        _shellRunner = shellRunner;
    }

    public async Task InstallChoco(EventHandler<string>? onInstallChocoScriptOutput = null)
    {
        await _shellRunner.RunScriptFromWeb(ChocoInstallUri, onInstallChocoScriptOutput);
    }

    public async Task InstallPackages<TProgress>(ProgressActionEvents<TProgress>? events)
    {
        if (events is not ProgressActionEvents<string> stringProgressEvents)
            throw new InvalidOperationException("Progress type must be string");

        await Task.Run(() =>
            _shellRunner.RunScript($"choco install {Constants.ChocoPackagesConfig} -y --acceptlicense --force",
                stringProgressEvents.Progress));
    }
}