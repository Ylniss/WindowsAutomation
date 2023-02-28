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
                _shellRunner.RunScript(
                    $"$qa = New-Object -ComObject shell.application\n$qa.NameSpace('{dir}').Self.InvokeVerb(\"pintohome\")");
            });
    }

    public void UnpinFromQuickAccess(string directory)
    {
        WhenUnpin.Act(directory, dir =>
        {
            _shellRunner.RunScript(
                """($qa.Namespace("shell:::{679F85CB-0220-4080-B29B-5540CC05AAB6}").Items() | Where-Object { $_.Path -EQ '""" +
                dir + """' }).InvokeVerb("unpinfromhome")"""
            );
        });
    }
}