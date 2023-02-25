namespace WindowsAutomation.Shared.Filesystem.DirCleaner;

public interface IDirCleaner
{
    void RemoveAllFilesInDir(string directory);
    void CleanRecycleBin();
}