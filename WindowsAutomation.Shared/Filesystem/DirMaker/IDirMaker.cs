using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Filesystem.DirMaker;

public interface IDirMaker
{
    RxEvent<string> WhenMake { get; }
    RxEvent<(string source, string destination)> WhenShortcutMake { get; }
    void MakeDirForFileIfNotExists(string fileDestination);
    void MakeDirIfNotExists(string path);
    void MakeDirShortcut(string source, string destination);
}