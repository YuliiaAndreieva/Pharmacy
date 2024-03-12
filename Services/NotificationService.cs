using WepPha2.Data;
using WepPha2.Models;

namespace WepPha2.Services;

public class NotificationService : IObservable
{
    public List<IObserver> Observers = new List<IObserver>();
    public void Attach(
        IObserver observer)
    {
        Observers.Add(observer);
    }

    public void Detach(
        IObserver observer)
    {
        Observers.Remove(observer);
    }

    public void Notify(
        Medicine medicine)
    {
        foreach (var observer in Observers)
        {
            observer.Update(medicine);
        }
    }
    public void CheckMedicine(
        IEnumerable<Medicine> medicines)
    {
        foreach (var medicine in medicines.Where(medicine => medicine.Quantity == 0))
        {
            Notify(medicine);
        }
    }
}