using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell;

namespace WindowsAutomation.Shared.Shell;

public class PowerShellRunner : IShellRunner
{
    public void RunScript(string script, Action<string>? onOutput = null)
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

    private void RegisterOutputEventHandler(PSDataCollection<PSObject> output, Action<string> onOutput)
    {
        output.DataAdded += (sender, e) =>
        {
            if (sender is not PSDataCollection<PSObject> records) return;
            var newRecord = records[e.Index];

            onOutput.Invoke(newRecord.BaseObject.ToString() ?? string.Empty);
        };
    }
}