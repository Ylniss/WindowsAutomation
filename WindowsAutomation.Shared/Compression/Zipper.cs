using System.IO.Compression;

namespace WindowsAutomation.Shared.Compression;

public class Zipper : IZipper
{
    public void Extract(string zipPath, string destination, bool overwrite = true, bool removeZip = true)
    {
        using var archive = ZipFile.OpenRead(zipPath);
        foreach (var entry in archive.Entries)
            entry.ExtractToFile(Path.Combine(destination, entry.FullName), overwrite);

        archive.Dispose();
        if (removeZip) File.Delete(zipPath);
    }
}