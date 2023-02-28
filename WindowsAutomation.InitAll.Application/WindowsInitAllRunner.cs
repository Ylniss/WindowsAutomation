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

    private readonly IFileSerializer _fileSerializer;

    public WindowsInitAllRunner(IEnumerable<IPackageInstaller> packageInstallers, IDirCleaner dirCleaner,
        IDirMaker dirMaker, IDirCopier dirCopier, IGitClient gitClient, IFileSerializer fileSerializer,
        ICursorChanger cursorChanger, IPinner pinner)
    {
        PackageInstallers = packageInstallers;

        DirCleaner = dirCleaner;
        DirMaker = dirMaker;
        DirCopier = dirCopier;

        GitClient = gitClient;
        CursorChanger = cursorChanger;
        Pinner = pinner;
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

    public void CopyDirectories(CopyPaths[] copyPaths)
    {
        foreach (var copyPath in copyPaths) DirCopier.CopyDirectory(copyPath.From, copyPath.To);
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