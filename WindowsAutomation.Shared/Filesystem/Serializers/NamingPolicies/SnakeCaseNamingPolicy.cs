using System.Text.Json;

namespace WindowsAutomation.Shared.Filesystem.Serializers.NamingPolicies;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) =>
        string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
}