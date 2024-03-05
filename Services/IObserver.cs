using WepPha2.Models;
using WepPha2.Services.SmsSending;

namespace WepPha2.Services;

public interface IObserver
{
    Task<bool> Update(Medicine medicine);
}