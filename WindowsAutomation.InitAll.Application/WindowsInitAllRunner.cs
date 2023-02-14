using WindowsAutomation.InitAll.Application.Installers;

namespace WindowsAutomation.InitAll.Application;

public class WindowsInitAllRunner : IInitAllRunner
{
    public Action? BeforePackageStatusSet { get; set; }
    public Action<string, bool>? OnPackageStatusSet { get; set; }
    public Action? OnPackageNotFound { get; set; }
    public Func<string, bool>? AskQuestionYesNoToContinueOnNotFoundPackages { get; set; }
    public Action? BeforeInstallPackages { get; set; }
    public Action<string>? OnPackageInstall { get; set; }
    public Action? BeforeExitInitRunner { get; set; }

    private readonly IInstaller _installer;

    public WindowsInitAllRunner(IInstaller installer)
    {
        _installer = installer;
    }

    public async Task RunCoreLogic(IServiceProvider provider)
    {
        BeforePackageStatusSet?.Invoke();
        var allPackagesFound = await _installer.CheckPackages(OnPackageStatusSet);

        if (!allPackagesFound)
        {
            OnPackageNotFound?.Invoke();
            var yes = AskQuestionYesNoToContinueOnNotFoundPackages?.Invoke("Wish to continue? (y/n):");
            if (yes is not null or false)
            {
                BeforeExitInitRunner?.Invoke();
                return;
            }

            BeforeInstallPackages?.Invoke();
            await _installer.InstallPackages(OnPackageInstall);
        }
    }
}