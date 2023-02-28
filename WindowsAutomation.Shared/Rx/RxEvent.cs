using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace WindowsAutomation.Shared.Rx;

public class RxEvent<T>
{
    private readonly Subject<T> _started = new();
    public IObservable<T> Started => _started.AsObservable();

    private readonly Subject<T> _finished = new();
    public IObservable<T> Finished => _finished.AsObservable();

    private readonly Subject<Exception> _error = new();
    public IObservable<Exception> Error => _error.AsObservable();

    public void Act(T argument, Action<T> action)
    {
        _started.OnNext(argument);

        try
        {
            action(argument);
        }
        catch (Exception ex)
        {
            _error.OnNext(ex);
            return;
        }

        _finished.OnNext(argument);
    }
}