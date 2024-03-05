using Microsoft.EntityFrameworkCore;
using WepPha2.Data;
using WepPha2.Interfaces;
using WepPha2.Models;
using WepPha2.Services.MailSending;
using IMailService = MailKit.IMailService;

namespace WepPha2.Services;

public class EmailObserver : IEmailObserver
{
    private readonly MailService _mailService;
    private readonly IServiceProvider _serviceProvider;

    public EmailObserver(
        MailService mailService,
        IServiceProvider serviceProvider)
    {
        _mailService = mailService;
        _serviceProvider = serviceProvider;
    }
    public async Task<bool> Update(
        Medicine medicine)
    {
        using var scope = _serviceProvider.CreateScope();
        var supplierRepository = scope.ServiceProvider.GetRequiredService<ISupplierRepository>();
        var supplier = await supplierRepository.GetSupplierById(medicine.SupplierId);
        MailData mailData = new MailData(supplier.CompanyName, supplier.EmailAddress, medicine.MedicineName, medicine.BatchNumber);
        return _mailService.SendMail(mailData);
    }
}