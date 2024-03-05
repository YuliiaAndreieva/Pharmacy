namespace WepPha2.Services.SmsSending;

public class SmsData
{
    public string To { get; set; }
    public string From { get; set; }
    public string SmsBody { get; set; }

    public SmsData(string supplierName, string supplierPhone, string medicineName, int batchNumber)
    {
        To = supplierPhone;
        From = "WebPha";
        SmsBody = $"Dear {supplierName}, Unfortunately, the product {medicineName} with batch number {batchNumber} has run out of stock in our pharmacy. We are looking forward to your delivery!";
    }
}