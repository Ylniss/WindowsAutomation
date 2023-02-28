using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Os.Windows.Pinner;

public interface IPinner
{
    RxEvent<string> WhenPin { get; }
    RxEvent<string> WhenUnpin { get; }
    void PinToQuickAccess(string directory);
    void UnpinFromQuickAccess(string directory);
}