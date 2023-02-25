using System.Text.Json;
using WindowsAutomation.Shared.Serializers.NamingPolicies;

namespace WindowsAutomation.Shared.Serializers;

public class JsonFileSerializer : IFileSerializer
{
    public T? DeserializeFromFile<T>(string path)
    {
        using var sr = new StreamReader(path);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        };

        var json = sr.ReadToEnd();
        return JsonSerializer.Deserialize<T>(json, options);
    }
}