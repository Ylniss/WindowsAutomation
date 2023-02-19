namespace WindowsAutomation.Shared.Events;

public record ProgressActionEvents<T>(EventHandler<string>? Before,
    EventHandler<T>? Progress,
    EventHandler<string>? Success,
    EventHandler<string>? Error);