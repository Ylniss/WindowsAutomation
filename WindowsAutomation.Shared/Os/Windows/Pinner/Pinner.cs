using WindowsAutomation.Shared.Rx;
using WindowsAutomation.Shared.Shell;

namespace WindowsAutomation.Shared.Os.Windows.Pinner;

public class Pinner : IPinner
{
    private readonly IShellRunner _shellRunner;
    public RxEvent<string> WhenPin { get; } = new();
    public RxEvent<string> WhenUnpin { get; } = new();

    public Pinner(IShellRunner shellRunner)
    {
        _shellRunner = shellRunner;
    }

    public void PinToQuickAccess(string directory)
    {
        WhenPin.Act(directory,
            dir =>
            {
                var pinScript = $"""
                        $qa = New-Object -ComObject shell.application
                        $qa.NameSpace('{dir}').Self.InvokeVerb("pintohome")
                    """;
                _shellRunner.RunScript(pinScript);
            });
    }

    public void UnpinFromQuickAccess(string directory)
    {
        WhenUnpin.Act(directory, dir =>
        {
            var unpinScript = """
                        $qa = New-Object -ComObject shell.application 
                        ($qa.Namespace("shell:::{679f85cb-0220-4080-b29b-5540cc05aab6}").Items() | where {$_.Path -eq "
                    """ + dir + """
                        "}).InvokeVerb("unpinfromhome")
                    """;
            _shellRunner.RunScript(unpinScript);
        });
    }
}