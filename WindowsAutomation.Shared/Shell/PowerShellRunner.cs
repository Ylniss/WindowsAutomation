using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell;
using WindowsAutomation.Shared.Web;

namespace WindowsAutomation.Shared.Shell;

public class PowerShellRunner : IShellRunner
{
    private readonly IWebDownloader _webDownloader;

    public PowerShellRunner(IWebDownloader webDownloader)
    {
        _webDownloader = webDownloader;
    }

    public void RunScript(string script, EventHandler<string>? onOutput = null)
    {
        var initialSessionState = InitialSessionState.CreateDefault();
        initialSessionState.ExecutionPolicy = ExecutionPolicy.Unrestricted;

        using var powerShell = PowerShell.Create(initialSessionState).AddScript(script);

        var output = new PSDataCollection<PSObject>();

        if (onOutput is not null)
            RegisterOutputEventHandler(output, onOutput);

        var result = powerShell.BeginInvoke<PSObject, PSObject>(null, output);
        result.AsyncWaitHandle.WaitOne();
    }

    public async Task RunScriptFromWeb(string scriptUri, EventHandler<string>? onOutput = null)
    {
        var chocoInstallationScript = await _webDownloader.DownloadContent(scriptUri);
        RunScript(chocoInstallationScript, onOutput);
    }

    private void RegisterOutputEventHandler(PSDataCollection<PSObject> output, EventHandler<string> onOutput)
    {
        output.DataAdded += (sender, e) =>
        {
            if (sender is not PSDataCollection<PSObject> records) return;
            var newRecord = records[e.Index];

            onOutput.Invoke(this, newRecord.BaseObject.ToString() ?? string.Empty);
        };
    }
}