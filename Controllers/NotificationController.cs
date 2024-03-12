using Microsoft.AspNetCore.Mvc;
using WepPha2.Interfaces;
using WepPha2.Services;
using WepPha2.ViewModels;

namespace WepPha2.Controllers;

public class NotificationController : Controller
{
    private readonly IMedicineRepository _medicineRepository;
    private readonly NotificationService _notificationService;
    private readonly EmailObserver _emailObserver;
    private readonly SmsObserver _smsObserver;

    public NotificationController(
        IMedicineRepository medicineRepository,
        NotificationService notificationService,
        EmailObserver emailObserver,
        SmsObserver smsObserver)
    {
        _medicineRepository = medicineRepository;
        _notificationService = notificationService;
        _emailObserver = emailObserver;
        _smsObserver = smsObserver;
    }

    public async Task<IActionResult> Index()
    {
        return View("ChangeNotification");
    }
    [HttpPost]
    public async Task<IActionResult> ChangeNotification(ChoiseViewModel vm)
    {
        if (vm.EmailNotification)
            _notificationService.Attach(_emailObserver);
        else
            _notificationService.Detach(_emailObserver);
        if (vm.SmsNotification)
            _notificationService.Attach(_smsObserver);
        else
            _notificationService.Detach(_smsObserver);

        return View();
    }
    public async Task<IActionResult> Notificate()
    {
        var medicines = await _medicineRepository.GetAll();
        
        _notificationService.CheckMedicine(medicines);
        
        return RedirectToAction("Index", "Supplier");
    }

}
