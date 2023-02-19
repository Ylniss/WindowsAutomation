namespace WindowsAutomation.Shared.Shell;

public interface IShellRunner
{
    void RunScript(string script, EventHandler<string>? onOutput = null);
    Task RunScriptFromWeb(string scriptUri, EventHandler<string>? onOutput = null);
}