using System.Text.Json;

namespace WindowsAutomation.Shared;

public static class Serialization
{
    public static T DeserializeFromFile<T>(string path)
    {
        using var sr = new StreamReader(path);

        var json = sr.ReadToEnd();
        return JsonSerializer.Deserialize<T>(json);
    }
}