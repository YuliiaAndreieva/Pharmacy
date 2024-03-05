using WepPha2.Models;

namespace WepPha2.Services;

public interface ISmsObserver : IObserver
{
    Task<bool> Update(
        Medicine medicine);
}