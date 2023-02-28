using System.Text.Json;
using System.Text.Json.Serialization;
using WindowsAutomation.Shared.Os.Windows;

namespace WindowsAutomation.Shared.Filesystem.Serializers.Converters;

public class ThemeJsonConverter : JsonConverter<Theme>
{
    public override Theme Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var themeString = reader.GetString();
        themeString = themeString?.ToLower();
        return themeString switch
        {
            "light" => Theme.Light,
            _ => Theme.Dark
        };
    }

    public override void Write(Utf8JsonWriter writer, Theme value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().ToLower());
    }
}