using WepPha2.Models;
using WepPha2.Interfaces;
using WepPha2.Services.SmsSending;

namespace WepPha2.Services;

public class SmsObserver : ISmsObserver
{
    private readonly SmsService _smsService;
    private readonly IServiceProvider _serviceProvider;

    public SmsObserver(IServiceProvider serviceProvider,
        SmsService smsService)
    {
        _serviceProvider = serviceProvider;
        _smsService = smsService;
    }
    public async Task<bool> Update(
        Medicine medicine)
    {
        using var scope = _serviceProvider.CreateScope();
        var supplierRepository = scope.ServiceProvider.GetRequiredService<ISupplierRepository>();
        var supplier = await supplierRepository.GetSupplierById(medicine.SupplierId);
        SmsData smsData = new SmsData(supplier.CompanyName,supplier.MobilePhone, medicine.MedicineName, medicine.BatchNumber);
        _smsService.SendSms(smsData);
        return default;
    }
}