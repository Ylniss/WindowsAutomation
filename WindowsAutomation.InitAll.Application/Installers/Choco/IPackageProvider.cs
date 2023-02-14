namespace WindowsAutomation.InitAll.Application.Installers.Choco;

public interface IPackageProvider
{
    IEnumerable<string> LoadPackages();
}