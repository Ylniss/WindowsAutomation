using WindowsAutomation.Shared.RegularExpression.Dtos;
using WindowsAutomation.Shared.Web.Dtos;

namespace WindowsAutomation.Shared.Web;

public interface IWebDownloader
{
    Task<string> DownloadContent(string uri);
    Task DownloadFile(WebFileDownload webFileDownload);
    Task ExtractLinkAndDownloadFile(WebFileDownload webFileDownload, RegexGroupNameMatch regex);
}