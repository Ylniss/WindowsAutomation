using CliWrap;
using WindowsAutomation.Shared;

namespace WindowsAutomation.InitAll.Application.Installers.Choco;

public class ChocoAppsInstaller
{
    private const string _packagesFileName = "choco_packages.json";
    private readonly string[] _packages;

    private readonly IDictionary<string, bool> _packageFindStatuses = new Dictionary<string, bool>();

    public ChocoAppsInstaller()
    {
        _packages = LoadPackagesFromJsonFile();
    }

    public async Task<bool> CheckPackages(Action<KeyValuePair<string, bool>>? afterCheck = null)
    {
        var anyNotFound = true;

        var findPackageTasks = _packages.Select(ChocoFindPackage).ToList();

        while (findPackageTasks.Any())
        {
            var finishedTask = await Task.WhenAny(findPackageTasks);
            findPackageTasks.Remove(finishedTask);
            var (package, result) = await finishedTask;

            var found = result.ExitCode == 0;
            if (!found) anyNotFound = found;
            afterCheck?.Invoke(new KeyValuePair<string, bool>(package, found));
        }

        return anyNotFound;
    }

    private async Task<(string package, CommandResult result)> ChocoFindPackage(string package)
    {
        var chocoCmd = Cli.Wrap("choco");
        return (package, await chocoCmd.WithArguments(new[] { "find", $"{package}" })
            .ExecuteAsync().Task);
    }

    private string[] LoadPackagesFromJsonFile()
    {
        var packagesPath = $"""{Constants.Paths.WorkingDir}\{_packagesFileName}""";
        if (!File.Exists(packagesPath))
            File.AppendAllText(packagesPath, "[]");

        return Serialization.DeserializeFromFile<string[]>(packagesPath);
    }
}