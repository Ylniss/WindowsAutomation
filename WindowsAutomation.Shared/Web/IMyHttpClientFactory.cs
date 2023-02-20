using System.Reactive.Subjects;

namespace WindowsAutomation.Shared.Web;

public interface IMyHttpClientFactory
{
    HttpClient CreateWithProgress(ISubject<double?>? whenDownloadProgressReceived = null,
        ISubject<double?>? whenUploadProgressReceived = null);
}