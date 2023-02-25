using System.Reactive;
using WindowsAutomation.InitAll.Application.PackageInstallers;

namespace WindowsAutomation.InitAll.Application;

public interface IInitAllRunner
{
    IEnumerable<IPackageInstaller> PackageInstallers { get; }

    IObservable<Unit> WhenDesktopFilesRemoveStarted { get; }
    Task RunCoreLogic();
}