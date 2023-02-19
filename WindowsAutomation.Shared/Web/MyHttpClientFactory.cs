using System.Net.Http.Handlers;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WindowsAutomation.Shared.Web.Extensions;

namespace WindowsAutomation.Shared.Web;

public class MyHttpClientFactory : IMyHttpClientFactory
{
    private readonly Subject<double?> _whenDownloadProgressReceived = new();
    private readonly Subject<double?> _whenUploadProgressReceived = new();

    public IObservable<double?> WhenDownloadProgressReceived => _whenDownloadProgressReceived.AsObservable();
    public IObservable<double?> WhenUploadProgressReceived => _whenUploadProgressReceived.AsObservable();

    public HttpClient CreateWithProgress()
    {
        var handler = new HttpClientHandler { AllowAutoRedirect = true };
        var progressHandler = new ProgressMessageHandler(handler);

        progressHandler.HttpReceiveProgress += (_, args) =>
        {
            _whenDownloadProgressReceived.OnNext(args.PreciseProgressPercentage());
        };

        progressHandler.HttpSendProgress += (_, args) =>
        {
            _whenUploadProgressReceived.OnNext(args.PreciseProgressPercentage());
        };

        return new HttpClient(progressHandler);
    }
}