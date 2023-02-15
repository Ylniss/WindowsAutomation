namespace WindowsAutomation.InitAll.Application.Installers;

public interface IPackageInstaller
{
    Task<bool> CheckPackages(Action<string, bool>? afterCheck = null);
    Task InstallPackages(Action<string>? onInstall = null);
}