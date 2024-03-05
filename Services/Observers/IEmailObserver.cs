using WepPha2.Models;

namespace WepPha2.Services;

public interface IEmailObserver : IObserver
{
    Task<bool> Update(
        Medicine medicine);
}