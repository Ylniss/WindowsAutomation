using System.Text.RegularExpressions;
using WindowsAutomation.Shared.RegularExpression.Dtos;

namespace WindowsAutomation.Shared.RegularExpression;

public class RegexExtractor : IRegexExtractor
{
    public string Extract(string input, RegexGroupNameMatch regex)
    {
        var match = Regex.Match(input, regex.Pattern, RegexOptions.IgnoreCase);
        return match.Groups[regex.GroupName].Value;
    }
}