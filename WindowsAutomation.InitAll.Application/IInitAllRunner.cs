namespace WindowsAutomation.InitAll.Application;

public interface IInitAllRunner
{
    public Action? BeforePackageStatusSet { get; set; }
    public Action<string, bool>? OnPackageStatusSet { get; set; }
    public Action? OnPackageNotFound { get; set; }
    public Func<string, bool>? AskQuestionYesNoToContinueOnNotFoundPackages { get; set; }
    public Action? BeforeInstallPackages { get; set; }
    public Action<string>? OnPackageInstall { get; set; }
    public Action? BeforeExitInitRunner { get; set; }

    Task RunCoreLogic(IServiceProvider provider);
}