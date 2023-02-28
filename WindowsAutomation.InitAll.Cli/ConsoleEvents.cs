using System.Reactive.Linq;
using WindowsAutomation.InitAll.Application;
using WindowsAutomation.InitAll.Application.PackageInstallers;
using WindowsAutomation.InitAll.Application.PackageInstallers.MyPackageInstaller;
using WindowsAutomation.Shared;

namespace WindowsAutomation.InitAll.Cli;

public static class ConsoleEvents
{
    public static void SetupGit(IInitAllRunner initAllRunner)
    {
        initAllRunner.GitClient.WhenGitClone.Started
            .Subscribe(repo => Console.Write($"Cloning repo '{repo.Name}' to '{repo.Destination}'..."));

        initAllRunner.GitClient.WhenGitClone.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.GitClient.WhenGitClone.Error
            .Subscribe(exception => CouldNot("clone repo", exception.Message));
    }

    public static void SetupOs(IInitAllRunner initAllRunner)
    {
        initAllRunner.CursorChanger.WhenCursorThemeSet.Started
            .Subscribe(theme => Console.Write($"Changing cursor theme to '{theme}'..."));

        initAllRunner.CursorChanger.WhenCursorThemeSet.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.CursorChanger.WhenCursorThemeSet.Error
            .Subscribe(exception => CouldNot("change theme", exception.Message));


        initAllRunner.Pinner.WhenPin.Started
            .Subscribe(dir => Console.Write($"Pinning '{dir}' to Quick Access..."));

        initAllRunner.Pinner.WhenPin.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.Pinner.WhenPin.Error
            .Subscribe(exception => CouldNot("pin", exception.Message));

        initAllRunner.Pinner.WhenUnpin.Started
            .Subscribe(dir => Console.Write($"Unpinning'{dir}' from Quick Access..."));

        initAllRunner.Pinner.WhenUnpin.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.Pinner.WhenUnpin.Error
            .Subscribe(exception => CouldNot("unpin", exception.Message));
    }

    public static void SetupFilesystem(IInitAllRunner initAllRunner)
    {
        initAllRunner.DirCleaner.WhenRemove.Started
            .Where(dir => !dir.Contains(Constants.ProfileName))
            .Subscribe(dir => Console.Write($"Removing files in '{dir}'..."));

        initAllRunner.DirCleaner.WhenRemove.Finished
            .Where(dir => !dir.Contains(Constants.ProfileName))
            .Subscribe(_ => DoneMessage());

        initAllRunner.DirCleaner.WhenRemove.Error
            .Subscribe(exception => CouldNot("remove files", exception.Message));


        initAllRunner.DirMaker.WhenMake.Started
            .Subscribe(dir => Console.Write($"Creating directory: '{dir}'..."));

        initAllRunner.DirMaker.WhenMake.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.DirMaker.WhenMake.Error
            .Subscribe(exception => CouldNot("create directory", exception.Message));


        initAllRunner.DirMaker.WhenShortcutMake.Started
            .Subscribe(paths => Console.Write($"Creating shortcut from '{paths.source}' to '{paths.destination}'..."));

        initAllRunner.DirMaker.WhenShortcutMake.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.DirMaker.WhenShortcutMake.Error
            .Subscribe(exception => CouldNot("create shortcut", exception.Message));


        initAllRunner.DirCopier.WhenCopy.Started
            .Subscribe(dirs => Console.Write($"Copying from '{dirs.source}' to '{dirs.destination}'..."));

        initAllRunner.DirCopier.WhenCopy.Finished
            .Subscribe(_ => DoneMessage());

        initAllRunner.DirCopier.WhenCopy.Error
            .Subscribe(exception => CouldNot("create directory, not found", exception.Message));
    }

    public static void SetupGeneralInstaller(IPackageInstaller? packageInstaller)
    {
        packageInstaller?.WhenDownloadStarted?
            .Subscribe(uri => Console.WriteLine($"Download from '{uri}' started..."));

        packageInstaller?.WhenDownloadProgressReceived?
            .Where(progress => progress is not null)
            .Subscribe(progress =>
                Console.Write($"\rDownload progress: {progress * 100: 0.00}%"));
    }

    public static void SetupMyAppsInstaller(MyPackageInstaller? myAppsInstaller)
    {
        myAppsInstaller?.WhenInstallStarted
            .Where(installationStep => installationStep.Step == InstallationStep.Download)
            .Subscribe(step =>
                Console.WriteLine($"\n{step.Package} download started..."));

        myAppsInstaller?.WhenInstallStarted
            .Where(installationStep => installationStep.Step == InstallationStep.Extract)
            .Where(installationStep => installationStep.Destination is not null)
            .Subscribe(step =>
                Console.WriteLine($"\nExtracting '{step.Package}' to '{step.Destination}'"));

        myAppsInstaller?.WhenInstallStarted
            .Where(installationStep => installationStep.Step == InstallationStep.RunSetup)
            .Subscribe(step =>
                Console.Write($"\n{step.Package} installation started... "));

        myAppsInstaller?.WhenSetupOutputReceived
            .Where(output => !string.IsNullOrEmpty(output))
            .Subscribe(Console.WriteLine);

        myAppsInstaller?.WhenInstallStarted
            .Subscribe(_ => { }, DoneMessage);
    }

    public static void SetupChocoAppsInstaller(ChocoPackageInstaller? chocoAppsInstaller)
    {
        chocoAppsInstaller?.WhenInstallStarted
            .Where(installationStep => installationStep.Package.Contains(Constants.ChocoPackagesConfig))
            .Subscribe(installationStep =>
                Console.WriteLine($"Installation from {installationStep.Package} started..."));

        chocoAppsInstaller?.WhenInstallStarted
            .Where(installationStep => !installationStep.Package.Contains(Constants.ChocoPackagesConfig))
            .Subscribe(config =>
                Console.WriteLine("Downloading chocolatey..."));

        chocoAppsInstaller?.WhenSetupOutputReceived
            .Where(output => output.Contains("Progress: "))
            .Subscribe(output => Console.Write($"\r{output}"));

        chocoAppsInstaller?.WhenSetupOutputReceived
            .Where(output => !output.Contains("Progress: "))
            .Subscribe(Console.WriteLine);
    }

    private static void CouldNot(string action, string message)
    {
        Console.WriteLine($" Could not {action}: '{message}'");
    }

    private static void DoneMessage()
    {
        Console.WriteLine(" Done.");
    }
}