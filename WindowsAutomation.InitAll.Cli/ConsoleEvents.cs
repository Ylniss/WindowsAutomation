using System.Reactive.Linq;
using WindowsAutomation.InitAll.Application;
using WindowsAutomation.InitAll.Application.PackageInstallers;
using WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller;
using WindowsAutomation.Shared;

namespace WindowsAutomation.InitAll.Cli;

public class ConsoleEvents
{
    private readonly List<string> _errors = new();

    public bool AnyErrors => _errors.Any();

    public void SetupGit(IInitAllRunner initAllRunner)
    {
        initAllRunner.GitClient.WhenGitClone.Started
            .Subscribe(repo => Console.Write($"Cloning repo '{repo.Name}' to '{repo.Destination}'..."));

        initAllRunner.GitClient.WhenGitClone.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.GitClient.WhenGitClone.Error
            .Subscribe(exception => CouldNot("clone repo", exception));
    }

    public void SetupOs(IInitAllRunner initAllRunner)
    {
        initAllRunner.CursorChanger.WhenCursorThemeSet.Started
            .Subscribe(theme => Console.Write($"Changing cursor theme to '{theme}'..."));

        initAllRunner.CursorChanger.WhenCursorThemeSet.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.CursorChanger.WhenCursorThemeSet.Error
            .Subscribe(exception => CouldNot("change theme", exception));


        initAllRunner.Pinner.WhenPin.Started
            .Subscribe(dir => Console.Write($"Pinning '{dir}' to Quick Access..."));

        initAllRunner.Pinner.WhenPin.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.Pinner.WhenPin.Error
            .Subscribe(exception => CouldNot("pin", exception));

        initAllRunner.Pinner.WhenUnpin.Started
            .Subscribe(dir => Console.Write($"Unpinning'{dir}' from Quick Access..."));

        initAllRunner.Pinner.WhenUnpin.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.Pinner.WhenUnpin.Error
            .Subscribe(exception => CouldNot("unpin", exception));


        initAllRunner.StartupAppsAdder.WhenAppStartupAdd.Started
            .Subscribe(path => Console.Write($"Adding {path} to system startup..."));

        initAllRunner.StartupAppsAdder.WhenAppStartupAdd.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.StartupAppsAdder.WhenAppStartupAdd.Error
            .Subscribe(exception => CouldNot("add app to startup", exception));


        initAllRunner.SystemDateTimeChanger.WhenTimeZoneChange.Started
            .Subscribe(timeZoneId => Console.Write($"Changing time zone to '{timeZoneId}'..."));

        initAllRunner.SystemDateTimeChanger.WhenTimeZoneChange.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.SystemDateTimeChanger.WhenTimeZoneChange.Error
            .Subscribe(exception => CouldNot("change time zone", exception));


        initAllRunner.SystemDateTimeChanger.WhenFormatChange.Started
            .Subscribe(data => Console.Write($"Changing {data.locale} format to '{data.format}'..."));

        initAllRunner.SystemDateTimeChanger.WhenFormatChange.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.SystemDateTimeChanger.WhenFormatChange.Error
            .Subscribe(exception =>
                CouldNot("change format", exception));
    }

    public void SetupFilesystem(IInitAllRunner initAllRunner)
    {
        initAllRunner.DirCleaner.WhenRemove.Started
            .Where(dir => !dir.Contains(Constants.ProfileName))
            .Subscribe(dir => Console.Write($"Removing files in '{dir}'..."));

        initAllRunner.DirCleaner.WhenRemove.Finished
            .Where(dir => !dir.Contains(Constants.ProfileName))
            .Subscribe(_ => DoneMessage());

        initAllRunner.DirCleaner.WhenRemove.Error
            .Subscribe(exception => CouldNot("remove files", exception));


        initAllRunner.DirMaker.WhenMake.Started
            .Subscribe(dir => Console.Write($"Creating directory: '{dir}'..."));

        initAllRunner.DirMaker.WhenMake.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.DirMaker.WhenMake.Error
            .Subscribe(exception => CouldNot("create directory", exception));


        initAllRunner.DirMaker.WhenShortcutMake.Started
            .Subscribe(paths => Console.Write($"Creating shortcut from '{paths.source}' to '{paths.destination}'..."));

        initAllRunner.DirMaker.WhenShortcutMake.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.DirMaker.WhenShortcutMake.Error
            .Subscribe(exception => CouldNot("create shortcut", exception));


        initAllRunner.DirCopier.WhenCopy.Started
            .Subscribe(dirs => Console.Write($"Copying from '{dirs.source}' to '{dirs.destination}'..."));

        initAllRunner.DirCopier.WhenCopy.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.DirCopier.WhenCopy.Error
            .Subscribe(exception => CouldNot("create directory, not found", exception));
    }

    public void SetupGeneralInstaller(IPackageInstaller? packageInstaller)
    {
        packageInstaller?.WhenDownloadStarted?
            .Subscribe(uri => Console.WriteLine($"Download from '{uri}' started..."));

        packageInstaller?.WhenDownloadProgressReceived?
            .Where(progress => progress is not null)
            .Subscribe(progress =>
                Console.Write($"\rDownload progress: {progress * 100: 0.00}%"));
    }

    public void SetupMyAppsInstaller(MyPackageInstaller? myAppsInstaller)
    {
        myAppsInstaller?.WhenInstall.Started
            .Where(installationStep => installationStep.Step == InstallationStep.Download)
            .Subscribe(step =>
                Console.WriteLine($"\n{step.Package} download started..."));

        myAppsInstaller?.WhenInstall.Started
            .Where(installationStep => installationStep.Step == InstallationStep.Extract)
            .Where(installationStep => installationStep.Destination is not null)
            .Subscribe(step =>
                Console.Write($"\nExtracting '{step.Package}' to '{step.Destination}'..."));

        myAppsInstaller?.WhenInstall.Started
            .Where(installationStep => installationStep.Step == InstallationStep.RunSetup)
            .Subscribe(step =>
                Console.Write($"\n{step.Package} installation started... "));

        myAppsInstaller?.WhenInstall.Finished
            .Subscribe(_ => DoneMessage());

        myAppsInstaller?.WhenInstall.Error
            .Subscribe(exception => CouldNot("finish installation", exception));

        myAppsInstaller?.WhenSetupOutputReceive.Started
            .Where(output => !string.IsNullOrEmpty(output))
            .Subscribe(Console.WriteLine);
    }

    public void SetupChocoAppsInstaller(ChocoPackageInstaller? chocoAppsInstaller)
    {
        chocoAppsInstaller?.WhenInstall.Started
            .Where(installationStep => installationStep.Package.Contains(Constants.ChocoPackagesConfig))
            .Subscribe(installationStep =>
                Console.WriteLine($"Installation from {installationStep.Package} started..."));

        chocoAppsInstaller?.WhenInstall.Started
            .Where(installationStep => !installationStep.Package.Contains(Constants.ChocoPackagesConfig))
            .Subscribe(config =>
                Console.WriteLine("Downloading chocolatey..."));

        chocoAppsInstaller?.WhenInstall.Finished
            .Subscribe(_ => DoneMessage());

        chocoAppsInstaller?.WhenInstall.Error
            .Subscribe(exception => CouldNot("finish installation", exception));

        chocoAppsInstaller?.WhenSetupOutputReceive.Started
            .Where(output => !output.Contains("Progress: "))
            .Where(output => !string.IsNullOrEmpty(output))
            .Subscribe(Console.WriteLine);

        chocoAppsInstaller?.WhenSetupOutputReceive.Started
            .Where(output => output.Contains("Progress: "))
            .Subscribe(output => Console.Write($"\r{output}"));

        chocoAppsInstaller?.WhenSetupOutputReceive.Started
            .Where(output => output.Contains("Progress: "))
            .Where(output => output.Contains("100%"))
            .Subscribe(output => Console.WriteLine($"\r{output}"));
    }

    private void CouldNot(string action, Exception exception)
    {
        var writeError = $" Could not {action}: '{exception.Message}'";
        if (exception.InnerException is not null)
            writeError += $", {exception.InnerException.Message}";
        Console.WriteLine(writeError);
        _errors.Add(writeError);
    }

    private static void DoneMessage()
    {
        Console.WriteLine(" Done.");
    }

    public void WriteErrors()
    {
        foreach (var error in _errors) Console.WriteLine(error);
    }
}