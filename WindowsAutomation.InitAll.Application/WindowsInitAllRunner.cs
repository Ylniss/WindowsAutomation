using LibGit2Sharp;
using WindowsAutomation.InitAll.Application.PackageInstallers;
using WindowsAutomation.Shared;
using WindowsAutomation.Shared.Filesystem.DirCleaner;
using WindowsAutomation.Shared.Filesystem.DirMaker;
using WindowsAutomation.Shared.Git;
using WindowsAutomation.Shared.Serializers;

namespace WindowsAutomation.InitAll.Application;

public class WindowsInitAllRunner : IInitAllRunner
{
    public IEnumerable<IPackageInstaller> PackageInstallers { get; }
    public IGitClient GitClient { get; }
    public IDirCleaner DirCleaner { get; }
    public IDirMaker DirMaker { get; }

    private readonly IFileSerializer _fileSerializer;

    public WindowsInitAllRunner(IEnumerable<IPackageInstaller> packageInstallers, IDirCleaner dirCleaner,
        IDirMaker dirMaker,
        IGitClient gitClient, IFileSerializer fileSerializer)
    {
        DirCleaner = dirCleaner;
        GitClient = gitClient;
        DirMaker = dirMaker;
        _fileSerializer = fileSerializer;
        PackageInstallers = packageInstallers;
    }

    public async Task RunCoreLogic()
    {
        var config = GetConfigFromJson();

        //foreach (var installer in PackageInstallers) await installer.InstallPackages();

        CloneReposFromGitHub(config.GithubCredentials, config.ReposToClone, config.Paths.Repo);
        SwapPowerShellProfileWithSymbolicLink($"""{config.Paths.Repo}\.dotfiles\{Constants.ProfileName}""");

        CreateInitialFolderStructure(config.FolderStructure);

        RemoveAllDesktopFiles();
        DirCleaner.CleanRecycleBin();
    }

    private InitAllConfig GetConfigFromJson()
    {
        var config =
            _fileSerializer.DeserializeFromFile<InitAllConfig>($"""{Environment.CurrentDirectory}\config.json""");

        if (config is null) throw new InvalidOperationException("config not initialized properly.");

        return config;
    }

    private void CloneReposFromGitHub(GithubCredentials githubCredentials, string[] repoNames, string repoPath)
    {
        GitClient.User = new UsernamePasswordCredentials
        {
            Username = githubCredentials.Username,
            Password = githubCredentials.AccessToken
        };

        foreach (var repoName in repoNames) GitClient.CloneIfNotExists(repoName, repoPath);
    }


    private void SwapPowerShellProfileWithSymbolicLink(string pathToTarget)
    {
        if (!File.Exists(pathToTarget)) return;

        DirCleaner.RemoveFileIfExists(Constants.CommonPaths.PowerShellProfile);
        DirMaker.MakeDirForFileIfNotExists(Constants.CommonPaths.PowerShellProfile);
        File.CreateSymbolicLink(Constants.CommonPaths.PowerShellProfile, pathToTarget);
    }

    private void CreateInitialFolderStructure(string[] directories)
    {
        // todo: replace ~ for user path in directories if doesnt work after test
        foreach (var directory in directories) DirMaker.MakeDirIfNotExists(directory);
    }

    private void RemoveAllDesktopFiles()
    {
        var commonDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
        DirCleaner.RemoveAllFilesInDir(commonDesktopPath);

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        DirCleaner.RemoveAllFilesInDir(desktopPath);
    }
}