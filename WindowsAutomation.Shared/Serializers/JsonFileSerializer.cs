using System.Text.Json;

namespace WindowsAutomation.Shared;

public class JsonFileSerializer : IFileSerializer
{
    public T? DeserializeFromFile<T>(string path)
    {
        using var sr = new StreamReader(path);

        var json = sr.ReadToEnd();
        return JsonSerializer.Deserialize<T>(json);
    }
}