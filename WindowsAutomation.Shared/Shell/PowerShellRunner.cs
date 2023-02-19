using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.PowerShell;
using WindowsAutomation.Shared.Web.Downloader;

namespace WindowsAutomation.Shared.Shell;

public class PowerShellRunner : IShellRunner
{
    private readonly IWebDownloader _webDownloader;

    private readonly Subject<string> _whenOutputReceived = new();
    public IObservable<string> WhenOutputReceived => _whenOutputReceived.AsObservable();
    public IObservable<string> WhenDownloadStarted { get; }
    public IObservable<double?> WhenDownloadProgressReceived { get; }


    public PowerShellRunner(IWebDownloader webDownloader)
    {
        _webDownloader = webDownloader;
        WhenDownloadStarted = _webDownloader.WhenDownloadStarted;
        WhenDownloadProgressReceived = _webDownloader.WhenDownloadProgressReceived;
    }

    public void RunScript(string script)
    {
        var initialSessionState = InitialSessionState.CreateDefault();
        initialSessionState.ExecutionPolicy = ExecutionPolicy.Unrestricted;

        using var powerShell = PowerShell.Create(initialSessionState).AddScript(script);

        PSDataCollection<PSObject> output = new();
        RegisterOutputEventHandler(output);

        var result = powerShell.BeginInvoke<PSObject, PSObject>(null, output);
        result.AsyncWaitHandle.WaitOne();
    }

    public async Task RunScriptFromWeb(string scriptUri)
    {
        var chocoInstallationScript = await _webDownloader.DownloadContent(scriptUri);
        RunScript(chocoInstallationScript);
    }

    private void RegisterOutputEventHandler(PSDataCollection<PSObject> output)
    {
        output.DataAdded += (sender, e) =>
        {
            if (sender is not PSDataCollection<PSObject> records) return;
            var newRecord = records[e.Index];

            _whenOutputReceived.OnNext(newRecord.BaseObject.ToString() ?? string.Empty);
        };
    }
}