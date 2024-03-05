namespace WepPha2.Services.MailSending;

public class MailSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string SenderEmail { get; set; }
    public string SenderPassword { get; set; }
    public string PharmacyEmail { get; set; }
    public string PharmacyName { get; set; }
}