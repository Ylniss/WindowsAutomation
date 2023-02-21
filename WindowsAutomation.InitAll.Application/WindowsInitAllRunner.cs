using WindowsAutomation.InitAll.Application.PackageInstallers;

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
        foreach (var installer in PackageInstallers) await installer.InstallPackages();

        //BeforeExitInitRunner?.Invoke(this, EventArgs.Empty);
    }
}