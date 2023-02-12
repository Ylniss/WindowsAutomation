using CliWrap;
using WindowsAutomation.Shared;

namespace WindowsAutomation.InitSetuper.Installers.Choco;

public class ChocoAppsInstaller
{
    private const string _packagesFileName = "choco_packages.json";
    private readonly string[] _packages;

    private readonly IDictionary<string, bool> _packageFindStatuses = new Dictionary<string, bool>();

    public ChocoAppsInstaller()
    {
        _packages = LoadPackages();
    }

    public async Task<bool> CheckPackages(Action<KeyValuePair<string, bool>>? afterCheck = null)
    {
        foreach (var package in _packages)
        {
            var chocoCmd = Cli.Wrap("choco");
            var result = await chocoCmd.WithArguments(new[] { "find", $"{package}" })
                .ExecuteAsync();

            var isOk = result.ExitCode == 0;
            _packageFindStatuses[package] = isOk;

            afterCheck?.Invoke(new KeyValuePair<string, bool>(package, isOk));
        }

        return _packageFindStatuses.Values.All(isOk => isOk);
    }

    private string[] LoadPackages()
    {
        var packagesPath = $"""{Constants.Paths.WorkingDir}\{_packagesFileName}""";
        if (!File.Exists(packagesPath))
            File.AppendAllText(packagesPath, "[]");

        return Serialization.DeserializeFromFile<string[]>(packagesPath);
    }
}