using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WindowsAutomation.InitAll.Application.PackageInstallers;
using WindowsAutomation.Shared.Filesystem.DirCleaner;

namespace WindowsAutomation.InitAll.Application;

public class WindowsInitAllRunner : IInitAllRunner
{
    private readonly IDirCleaner _dirCleaner;
    public IEnumerable<IPackageInstaller> PackageInstallers { get; }


    private readonly Subject<Unit> _whenDesktopFilesRemoveStarted = new();
    public IObservable<Unit> WhenDesktopFilesRemoveStarted => _whenDesktopFilesRemoveStarted.AsObservable();

    public WindowsInitAllRunner(IEnumerable<IPackageInstaller> packageInstallers, IDirCleaner dirCleaner)
    {
        _dirCleaner = dirCleaner;
        PackageInstallers = packageInstallers;
    }

    public async Task RunCoreLogic()
    {
        foreach (var installer in PackageInstallers) await installer.InstallPackages();

        DownloadLatestPowerShellProfileAndSetSymbolicLink();
        
        RemoveAllDesktopFiles();
        _dirCleaner.CleanRecycleBin();
    }

    private void DownloadLatestPowerShellProfileAndSetSymbolicLink()
    {
        
    }

    private void RemoveAllDesktopFiles()
    {
        _whenDesktopFilesRemoveStarted.OnNext(new Unit());
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
        _dirCleaner.RemoveAllFilesInDir(desktopPath);
    }
}