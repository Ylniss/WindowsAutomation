namespace WindowsAutomation.Shared;

public static class Constants
{
    public const string ProfileName = "Microsoft.PowerShell_profile.ps1";
    public const string ChocoPackagesConfig = "choco_packages.config";


    public static class Paths
    {
        public const string Repo = "C:/Repo";
        public const string Software = "C:/Downloads/Software";

        public static string ProgramFiles = Environment.ExpandEnvironmentVariables("%ProgramW6432%");
        public static string ProgramFilesX86 = Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%");

        public static string WorkingDir => Environment.CurrentDirectory;
    }
}