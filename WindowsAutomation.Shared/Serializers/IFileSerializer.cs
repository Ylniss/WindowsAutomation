namespace WindowsAutomation.Shared;

public interface IFileSerializer
{
    T? DeserializeFromFile<T>(string path);
}