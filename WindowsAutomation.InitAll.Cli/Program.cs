using WindowsAutomation.InitAll.Application.Installers.Choco;

Console.WriteLine(" ---------- Running Windows initialization script ---------- ");
try
{
    var chocoAppsInstaller = new ChocoAppsInstaller();
    Console.WriteLine("Checking choco_packages.json...");
    var allPackagesFound = await chocoAppsInstaller.CheckPackages(ConsoleWritePackageStatus);

    if (!allPackagesFound)
    {
        Console.WriteLine("Failed to find some packages.");

        //todo: pack mechanism into shared method
        Console.WriteLine("Wish to continue? (y/n):");
        var key = Console.ReadKey();
        if (key.Key == ConsoleKey.Y) return;
    }
}
catch (Exception e)
{
    Console.WriteLine($"Error occured during initialization:/n{e.Message}");
    throw;
}

void ConsoleWritePackageStatus(KeyValuePair<string, bool> status)
{
    Console.WriteLine($"{status.Key}: " + (status.Value ? "OK" : "NOT FOUND"));
}