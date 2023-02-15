namespace WindowsAutomation.Shared.Shell;

public interface IShellRunner
{
    IAsyncEnumerable<string> RunScript(string script);
}