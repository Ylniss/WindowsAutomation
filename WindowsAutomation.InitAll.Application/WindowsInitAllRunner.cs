using WindowsAutomation.InitAll.Application.Installers;
using WindowsAutomation.InitAll.Application.Installers.Choco;

namespace WindowsAutomation.InitAll.Application;

public class WindowsInitAllRunner : IInitAllRunner
{
    public Action? BeforeInstallChoco { get; set; }
    public Action<string>? OnInstallChocoOutput { get; set; }

    public Action? BeforePackageStatusSet { get; set; }
    public Action<string, bool>? OnPackageStatusSet { get; set; }

    public Action? OnPackageNotFound { get; set; }
    public Func<string, bool>? AskQuestionYesNoToContinueOnNotFoundPackages { get; set; }

    public Action? BeforeInstallPackages { get; set; }
    public Action<double>? OnPackageInstallProgress { get; set; }
    public Action<string>? OnPackageInstall { get; set; }

    public Action? BeforeExitInitRunner { get; set; }

    private readonly IPackageInstaller _packageInstaller;


    public WindowsInitAllRunner(IPackageInstaller packageInstaller)
    {
        _packageInstaller = packageInstaller;
    }

    public async Task RunCoreLogic()
    {
        if (_packageInstaller is ChocoAppsInstaller installer)
        {
            BeforeInstallChoco?.Invoke();
            await installer.InstallChoco(OnInstallChocoOutput);
        }


        BeforePackageStatusSet?.Invoke();
        var somePackagesNotFound = await _packageInstaller.CheckPackages(OnPackageStatusSet);

        if (somePackagesNotFound)
        {
            OnPackageNotFound?.Invoke();
            var yes = AskQuestionYesNoToContinueOnNotFoundPackages?.Invoke("Wish to continue?");
            if (yes is not null or false)
            {
                BeforeExitInitRunner?.Invoke();
                return;
            }
        }

        BeforeInstallPackages?.Invoke();
        _packageInstaller.InstallPackages(OnPackageInstallProgress, OnPackageInstall);
    }
}