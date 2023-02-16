namespace WindowsAutomation.InitAll.Application.Installers;

public interface IPackageInstaller
{
    Task<bool> CheckPackages(Action<string, bool>? afterCheck = null);
    void InstallPackages(Action<double>? progress = null, Action<string>? onInstall = null);
}