using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Filesystem.DirMaker;

public interface IDirMaker
{
    RxEvent<string> WhenMake { get; }
    void MakeDirForFileIfNotExists(string fileDestination);
    void MakeDirIfNotExists(string path);
}