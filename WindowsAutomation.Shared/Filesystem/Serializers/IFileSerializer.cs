namespace WindowsAutomation.Shared.Filesystem.Serializers;

public interface IFileSerializer
{
    T? DeserializeFromFile<T>(string path);
}