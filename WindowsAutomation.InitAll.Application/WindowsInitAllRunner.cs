using WindowsAutomation.InitAll.Application.Installers;

namespace WindowsAutomation.InitAll.Application;

public class WindowsInitAllRunner : IInitAllRunner
{
    public IEnumerable<IPackageInstaller> PackageInstallers { get; }

    public WindowsInitAllRunner(IEnumerable<IPackageInstaller> packageInstallers)
    {
        PackageInstallers = packageInstallers;
    }

    public async Task RunCoreLogic()
    {
        foreach (var installer in PackageInstallers)
            // if (installer is ChocoAppsInstaller chocoAppsInstaller)
            //     await chocoAppsInstaller.InstallChoco();
            await installer.InstallPackages();

        //BeforeExitInitRunner?.Invoke(this, EventArgs.Empty);
    }
}