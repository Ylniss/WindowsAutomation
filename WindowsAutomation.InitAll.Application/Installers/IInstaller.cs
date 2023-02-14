namespace WindowsAutomation.InitAll.Application.Installers;

public interface IInstaller
{
    Task<bool> CheckPackages(Action<string, bool>? afterCheck = null);
    Task InstallPackages(Action<string>? onInstall = null);
}