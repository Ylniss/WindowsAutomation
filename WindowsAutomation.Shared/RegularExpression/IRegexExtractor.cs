using WindowsAutomation.Shared.RegularExpression.Dtos;

namespace WindowsAutomation.Shared.RegularExpression;

public interface IRegexExtractor
{
    string Extract(string input, RegexGroupNameMatch regex);
}