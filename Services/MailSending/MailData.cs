namespace WepPha2.Services.MailSending;

public class MailData
{
    public string SupplierName { get; set; }
    public string SupplierEmail { get; set; }
    public string EmailSubject { get; set; }
    public string EmailBody { get; set; }

    public MailData(string supplierName, string supplierEmail, string medicineName, int batchNumber)
    {
        if (string.IsNullOrWhiteSpace(supplierName))
        {
            throw new ArgumentException("SupplierName cannot be null or empty.", nameof(supplierName));
        }

        SupplierName = supplierName;
        SupplierEmail = supplierEmail;
        EmailSubject = "Очікуємо вашу поставку !";
        EmailBody = $"На жаль, товар {medicineName} {batchNumber} закінчився в наявності у наші аптеці.";
    }
}
