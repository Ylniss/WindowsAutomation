namespace WindowsAutomation.Shared;

public static class Constants
{
    public const string ProfileName = "Microsoft.PowerShell_profile.ps1";
    public const string ChocoPackagesConfig = "choco_packages.config";


    public static class CommonPaths
    {
        public static string PowerShellProfile =
            $"""{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Documents\PowerShell\{ProfileName}""";

        public static string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        public static string ProgramFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        public static string User = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        public static string Software = $"""{User}\Downloads\Software""";
    }
}