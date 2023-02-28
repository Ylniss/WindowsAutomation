using WindowsAutomation.Shared.Rx;

namespace WindowsAutomation.Shared.Filesystem.DirCleaner;

public interface IDirCleaner
{
    RxEvent<string> WhenRemove { get; }
    void RemoveAllFilesInDir(string directory);
    void RemoveFileIfExists(string directory);
    void CleanRecycleBin();
}