namespace WindowsAutomation.Shared.Filesystem.DirCleaner;

public interface IDirCleaner
{
    public IObservable<string> WhenRemoveStarted { get; }
    void RemoveAllFilesInDir(string directory);
    void RemoveFileIfExists(string directory);
    void CleanRecycleBin();
}