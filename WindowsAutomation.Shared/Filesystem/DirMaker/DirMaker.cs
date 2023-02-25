namespace WindowsAutomation.Shared.Filesystem.DirMaker;

public class DirMaker : IDirMaker
{
    public void MakeDirForFileIfNotExists(string fileDestination)
    {
        var directory = Directory.GetParent(fileDestination)?.FullName;
        if (directory is not null) MakeDirIfNotExists(directory);
    }

    public void MakeDirIfNotExists(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
}