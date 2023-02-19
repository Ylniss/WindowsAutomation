namespace WindowsAutomation.Shared.Events;

public record ActionEvents(EventHandler<string>? Before,
    EventHandler<string>? Success,
    EventHandler<string>? Error);