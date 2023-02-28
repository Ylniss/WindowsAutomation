namespace WindowsAutomation.Shared.Os.Windows;

[Flags]
internal enum SysFlag : uint
{
    SpiSetcursors = 0x0057, // Windows API action to set the mouse cursor scheme
    SpifUpdateinifile = 0x01, // update the user profile settings in the Windows INI configuration file
    SpifSendchange = 0x02 // notify all top-level windows of a change to the user profile settings
}