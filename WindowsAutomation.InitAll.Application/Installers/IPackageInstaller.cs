using WindowsAutomation.Shared.Events;

namespace WindowsAutomation.InitAll.Application.Installers;

public interface IPackageInstaller
{
    Task InstallPackages<TProgress>(ProgressActionEvents<TProgress>? events);
}