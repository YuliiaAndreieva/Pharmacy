using WepPha2.Models;

namespace WepPha2.Services;

public interface IObservable
{
    void Attach(IObserver observer);
    
    void Detach(IObserver observer);
    
    void Notify(Medicine medicine);
}