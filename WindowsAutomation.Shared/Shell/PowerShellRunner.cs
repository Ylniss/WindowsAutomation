using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell;

namespace WindowsAutomation.Shared.Shell;

public class PowerShellRunner : IShellRunner
{
    public async IAsyncEnumerable<string> RunScript(string script)
    {
        var initialSessionState = InitialSessionState.CreateDefault();
        initialSessionState.ExecutionPolicy = ExecutionPolicy.Unrestricted;

        using var powerShell = PowerShell.Create(initialSessionState).AddScript(script);

        ICollection<PSObject> psOutput = await powerShell.InvokeAsync();

        foreach (var obj in psOutput)
            yield return
                $"{obj.Properties["Status"]} - {obj.Properties["DisplayName"].Value}";
    }
}