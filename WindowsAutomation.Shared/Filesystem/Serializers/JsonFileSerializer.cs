using System.Text.Json;
using WindowsAutomation.Shared.Filesystem.Serializers.Converters;
using WindowsAutomation.Shared.Filesystem.Serializers.NamingPolicies;

namespace WindowsAutomation.Shared.Filesystem.Serializers;

public class JsonFileSerializer : IFileSerializer
{
    public T? DeserializeFromFile<T>(string path)
    {
        using var sr = new StreamReader(path);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            Converters = { new ThemeJsonConverter() }
        };

        var json = sr.ReadToEnd();
        return JsonSerializer.Deserialize<T>(json, options);
    }
}