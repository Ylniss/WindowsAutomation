namespace WindowsAutomation.InitAll.Application;

public interface IInitAllRunner
{
    public event EventHandler? BeforeInstallChoco;
    public event EventHandler<string>? OnInstallChocoOutput;
    public event EventHandler? BeforeInstallPackages;
    public event EventHandler<string>? OnPackageInstall;
    public event EventHandler? BeforeExitInitRunner;
    public event EventHandler<double>? OnDownloadProgress;

    Task RunCoreLogic();
}