namespace WindowsAutomation.InitAll.Application.Installers;

public interface IPackageInstaller
{
    Task InstallPackages(EventHandler<string>? beforeDownload = null);
}