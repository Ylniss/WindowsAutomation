namespace WindowsAutomation.Shared.Filesystem.DirMaker;

public interface IDirMaker
{
    void MakeDirForFileIfNotExists(string fileDestination);
    void MakeDirIfNotExists(string path);
}