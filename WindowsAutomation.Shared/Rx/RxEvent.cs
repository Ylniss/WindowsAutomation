using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace WindowsAutomation.Shared.Rx;

public class RxEvent<T>
{
    public readonly Subject<T> StartedSubject = new();
    public IObservable<T> Started { get; private set; }


    public readonly Subject<T> FinishedSubject = new();
    public IObservable<T> Finished { get; private set; }


    public readonly Subject<Exception> ErrorSubject = new();
    public IObservable<Exception> Error { get; private set; }

    public RxEvent()
    {
        Started = StartedSubject.AsObservable();
        Finished = FinishedSubject.AsObservable();
        Error = ErrorSubject.AsObservable();
    }

    public void Merge(IEnumerable<RxEvent<T>> rxEvents)
    {
        rxEvents = rxEvents.ToList();
        Started = rxEvents.Select(rxEvent => rxEvent.Started).Merge();
        Finished = rxEvents.Select(rxEvent => rxEvent.Finished).Merge();
        Error = rxEvents.Select(rxEvent => rxEvent.Error).Merge();
    }

    public void Act(T argument, Action<T> action)
    {
        StartedSubject.OnNext(argument);

        try
        {
            action(argument);
        }
        catch (Exception ex)
        {
            ErrorSubject.OnNext(ex);
            return;
        }

        FinishedSubject.OnNext(argument);
    }

    public async Task ActAsync(T argument, Func<T, Task> action)
    {
        StartedSubject.OnNext(argument);

        try
        {
            await action(argument);
        }
        catch (Exception ex)
        {
            ErrorSubject.OnNext(ex);
            return;
        }

        FinishedSubject.OnNext(argument);
    }
}