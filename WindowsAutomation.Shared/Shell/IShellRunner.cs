namespace WindowsAutomation.Shared.Shell;

public interface IShellRunner
{
    void RunScript(string script, Action<string>? onProgress = null);
}