using System.Text.Json;

namespace WindowsAutomation.Shared.Serializers;

public class JsonFileSerializer : IFileSerializer
{
    public T? DeserializeFromFile<T>(string path)
    {
        using var sr = new StreamReader(path);

        var json = sr.ReadToEnd();
        return JsonSerializer.Deserialize<T>(json);
    }
}