using WindowsAutomation.Shared.RegularExpression;

namespace WindowsAutomation.Shared.Web;

public interface IWebDownloader
{
    Task<string> DownloadContent(string uri);
    Task DownloadFile(WebFileDownload webFileDownload);
    Task ExtractLinkAndDownloadFile(WebFileDownload webFileDownload, RegexGroupNameMatch regex);
}