namespace WindowsAutomation.InitAll.Application.PackageInstallers;

public enum InstallationStep
{
    Download,
    Extract,
    RunSetup,
    RunScript
}

public record PackageInstallationStep(string Package, InstallationStep Step, string? Destination = null);