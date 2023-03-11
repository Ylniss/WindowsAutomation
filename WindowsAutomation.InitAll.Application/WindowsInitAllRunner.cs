using LibGit2Sharp;
using WindowsAutomation.InitAll.Application.PackageInstallers;
using WindowsAutomation.Shared;
using WindowsAutomation.Shared.Filesystem.DirCleaner;
using WindowsAutomation.Shared.Filesystem.DirCopier;
using WindowsAutomation.Shared.Filesystem.DirMaker;
using WindowsAutomation.Shared.Filesystem.Serializers;
using WindowsAutomation.Shared.Git;
using WindowsAutomation.Shared.Os.Windows.CursorChanger;
using WindowsAutomation.Shared.Os.Windows.Pinner;
using WindowsAutomation.Shared.Os.Windows.StartupAppsAdder;
using WindowsAutomation.Shared.Os.Windows.StartupAppsRemover;
using WindowsAutomation.Shared.Os.Windows.SystemDateTimeChanger;

namespace WindowsAutomation.InitAll.Application;

public class WindowsInitAllRunner : IInitAllRunner
{
    public IEnumerable<IPackageInstaller> PackageInstallers { get; }

    public IDirCleaner DirCleaner { get; }
    public IDirMaker DirMaker { get; }
    public IDirCopier DirCopier { get; }

    public IGitClient GitClient { get; }
    public ICursorChanger CursorChanger { get; }
    public IPinner Pinner { get; }
    public IStartupAppsAdder StartupAppsAdder { get; }
    public IStartupAppsRemover StartupAppsRemover { get; }
    public ISystemDateTimeChanger SystemDateTimeChanger { get; }

    private readonly IFileSerializer _fileSerializer;

    public WindowsInitAllRunner(IEnumerable<IPackageInstaller> packageInstallers, IDirCleaner dirCleaner,
        IDirMaker dirMaker, IDirCopier dirCopier, IGitClient gitClient, IFileSerializer fileSerializer,
        ICursorChanger cursorChanger, IPinner pinner, IStartupAppsAdder startupAppsAdder,
        IStartupAppsRemover startupAppsRemover, ISystemDateTimeChanger systemDateTimeChanger)
    {
        PackageInstallers = packageInstallers;

        DirCleaner = dirCleaner;
        DirMaker = dirMaker;
        DirCopier = dirCopier;

        GitClient = gitClient;
        CursorChanger = cursorChanger;
        Pinner = pinner;
        StartupAppsAdder = startupAppsAdder;
        StartupAppsRemover = startupAppsRemover;
        SystemDateTimeChanger = systemDateTimeChanger;
        _fileSerializer = fileSerializer;
    }

    public InitAllConfig GetConfigFromJson()
    {
        var config =
            _fileSerializer.DeserializeFromFile<InitAllConfig>($"""{Environment.CurrentDirectory}\config.json""");

        if (config is null) throw new InvalidOperationException("config not initialized properly.");

        return config;
    }

    public async Task InstallPackages()
    {
        foreach (var installer in PackageInstallers) await installer.InstallPackages();
    }

    public void SetupStartupApplications(string[] startupApps, string[] startupAppsToRemove)
    {
        foreach (var app in startupApps) StartupAppsAdder.AddApp(app);
        foreach (var app in startupAppsToRemove) StartupAppsRemover.RemoveApp(app);
    }

    public void CloneReposFromGitHub(GithubCredentials githubCredentials, string[] repoNames, string repoPath)
    {
        GitClient.User = new UsernamePasswordCredentials
        {
            Username = githubCredentials.Username,
            Password = githubCredentials.AccessToken
        };

        foreach (var repoName in repoNames) GitClient.CloneIfNotExists(repoName, repoPath);
    }


    public void SwapPowerShellProfileWithSymbolicLink(string pathToTarget)
    {
        if (!File.Exists(pathToTarget)) return;

        DirCleaner.RemoveFileIfExists(Constants.CommonPaths.PowerShellProfile);
        DirMaker.MakeDirForFileIfNotExists(Constants.CommonPaths.PowerShellProfile);
        File.CreateSymbolicLink(Constants.CommonPaths.PowerShellProfile, pathToTarget);
    }

    public void CreateInitialFolderStructure(string[] directories)
    {
        foreach (var directory in directories) DirMaker.MakeDirIfNotExists(directory);
    }

    public void CopyDirectories(SourceTargetPaths[] copyPaths)
    {
        foreach (var copyPath in copyPaths) DirCopier.CopyDirectory(copyPath.From, copyPath.To);
    }

    public void CreateShortcuts(SourceTargetPaths[] paths)
    {
        foreach (var path in paths) DirMaker.MakeDirShortcut(path.From, path.To);
    }

    public void PinDirectoriesToQuickAccess(string[] directories)
    {
        Pinner.PinToQuickAccess(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
        Pinner.PinToQuickAccess(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        Pinner.PinToQuickAccess(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        Pinner.PinToQuickAccess(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
        Pinner.PinToQuickAccess(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));

        Pinner.UnpinFromQuickAccess(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
        Pinner.UnpinFromQuickAccess(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        Pinner.UnpinFromQuickAccess(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        Pinner.UnpinFromQuickAccess(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
        Pinner.UnpinFromQuickAccess(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));

        foreach (var directory in directories) Pinner.PinToQuickAccess(directory);
    }

    public void SetSystemDateTime(InitAllConfig config)
    {
        SystemDateTimeChanger.ChangeTimeZone(config.TimeZoneId);
        SystemDateTimeChanger.ChangeFormat(Locale.ShortDateFormat, config.DateTimeFormat.ShortDate);
        SystemDateTimeChanger.ChangeFormat(Locale.LongDateFormat, config.DateTimeFormat.LongDate);
        SystemDateTimeChanger.ChangeFormat(Locale.ShortTimeFormat, config.DateTimeFormat.ShortTime);
        SystemDateTimeChanger.ChangeFormat(Locale.LongTimeFormat, config.DateTimeFormat.LongTime);
        SystemDateTimeChanger.ChangeFormat(Locale.FirstDayOfWeek, config.DateTimeFormat.FirstDayOfWeek);
    }

    public void CleanDesktopAndRecycleBin()
    {
        var commonDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
        DirCleaner.RemoveAllFilesInDir(commonDesktopPath);

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        DirCleaner.RemoveAllFilesInDir(desktopPath);
        DirCleaner.CleanRecycleBin();
    }
}