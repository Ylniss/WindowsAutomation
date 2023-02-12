namespace WindowsAutomation.Shared;

public static class Constants
{
    public const string ProfileName = "Microsoft.PowerShell_profile.ps1";

    public static class Paths
    {
        public const string Repo = "C:/Repo";
        public const string Software = "C:/Downloads/Software";

        public static string WorkingDir => Environment.CurrentDirectory;
    }
}