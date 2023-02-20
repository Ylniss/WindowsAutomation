using System.Net.Http.Handlers;
using System.Reactive.Subjects;
using WindowsAutomation.Shared.Web.Extensions;

namespace WindowsAutomation.Shared.Web;

public class MyHttpClientFactory : IMyHttpClientFactory
{
    public HttpClient CreateWithProgress(ISubject<double?>? whenDownloadProgressReceived = null,
        ISubject<double?>? whenUploadProgressReceived = null)
    {
        var handler = new HttpClientHandler { AllowAutoRedirect = true };
        var progressHandler = new ProgressMessageHandler(handler);

        progressHandler.HttpReceiveProgress += (_, args) =>
        {
            whenDownloadProgressReceived?.OnNext(args.PreciseProgressPercentage());
        };

        progressHandler.HttpSendProgress += (_, args) =>
        {
            whenUploadProgressReceived?.OnNext(args.PreciseProgressPercentage());
        };

        return new HttpClient(progressHandler);
    }
}