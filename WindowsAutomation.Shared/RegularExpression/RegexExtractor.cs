using System.Text.RegularExpressions;

namespace WindowsAutomation.Shared.RegularExpression;

public record RegexGroupNameMatch(string Pattern, string GroupName);

public interface IRegexExtractor
{
    string Extract(string input, RegexGroupNameMatch regex);
}

public class RegexExtractor : IRegexExtractor
{
    public string Extract(string input, RegexGroupNameMatch regex)
    {
        var match = Regex.Match(input, regex.Pattern, RegexOptions.IgnoreCase);
        return match.Groups[regex.GroupName].Value;
    }
}