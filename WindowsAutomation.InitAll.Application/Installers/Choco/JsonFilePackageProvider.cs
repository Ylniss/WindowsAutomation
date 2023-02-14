using WindowsAutomation.Shared;

namespace WindowsAutomation.InitAll.Application.Installers.Choco;

public class JsonFilePackageProvider : IPackageProvider
{
    private const string _packagesFileName = "choco_packages.json";

    private readonly IFileSerializer _fileSerializer;

    public JsonFilePackageProvider(IFileSerializer fileSerializer)
    {
        _fileSerializer = fileSerializer;
    }

    public IEnumerable<string> LoadPackages()
    {
        var packagesPath = $"""{Constants.Paths.WorkingDir}\{_packagesFileName}""";
        if (!File.Exists(packagesPath))
            File.AppendAllText(packagesPath, "[]");

        return _fileSerializer.DeserializeFromFile<string[]>(packagesPath) ?? Array.Empty<string>();
    }
}