namespace WindowsAutomation.Shared.Filesystem.Compression;

public interface IZipper
{
    void Extract(string zipPath, string destination, bool overwrite = true, bool removeZip = true);
}