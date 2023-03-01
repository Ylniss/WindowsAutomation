using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Os.Windows.SystemDateTimeChanger;

public interface ISystemDateTimeChanger
{
    RxEvent<string> WhenTimeZoneChange { get; }
    void ChangeTimeZone(string timeZoneId);
}