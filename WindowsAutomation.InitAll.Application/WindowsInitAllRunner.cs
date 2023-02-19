using WindowsAutomation.InitAll.Application.Installers;

namespace WindowsAutomation.InitAll.Application;

public class WindowsInitAllRunner : IInitAllRunner
{
    public event EventHandler? BeforeInstallChoco;
    public event EventHandler<string>? OnInstallChocoOutput;
    public event EventHandler? BeforeInstallPackages;
    public event EventHandler<string>? OnPackageInstall;
    public event EventHandler? BeforeExitInitRunner;
    public event EventHandler<double>? OnDownloadProgress;


    private readonly IEnumerable<IPackageInstaller> _packageInstallers;

    public WindowsInitAllRunner(IEnumerable<IPackageInstaller> packageInstallers)
    {
        _packageInstallers = packageInstallers;
    }

    public async Task RunCoreLogic()
    {
        foreach (var installer in _packageInstallers)
        {
            if (installer is ChocoAppsInstaller chocoAppsInstaller)
            {
                BeforeInstallChoco?.Invoke(this, EventArgs.Empty);
                await chocoAppsInstaller.InstallChoco(OnInstallChocoOutput);
            }

            if (installer is MyAppsInstaller myAppsInstaller) myAppsInstaller.OnDownloadProgress += OnDownloadProgress;

            if (installer is not ChocoAppsInstaller) // IN THAT IF ONLY FOR TESTING
            {
                BeforeInstallPackages?.Invoke(this, EventArgs.Empty);
                await installer.InstallPackages(OnPackageInstall);
            }
        }

        BeforeExitInitRunner?.Invoke(this, EventArgs.Empty);
    }
}