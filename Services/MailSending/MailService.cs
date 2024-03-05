using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace WepPha2.Services.MailSending;

public class MailService 
{
    private readonly MailSettings _mailSettings;
    public MailService(IOptions<MailSettings> mailSettingsOptions)
    {
        _mailSettings = mailSettingsOptions.Value;
    }
    public bool SendMail(MailData mailData)
    {
        try
        {
            using (MimeMessage emailMessage = new MimeMessage())
            {
                MailboxAddress emailFrom = new MailboxAddress(_mailSettings.PharmacyName, _mailSettings.PharmacyEmail);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(mailData.SupplierName, mailData.SupplierEmail);
                emailMessage.To.Add(emailTo);
                
                emailMessage.Subject = mailData.EmailSubject;

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = mailData.EmailBody;

                emailMessage.Body = emailBodyBuilder.ToMessageBody();
               
                using (SmtpClient mailClient = new SmtpClient())
                {
                    mailClient.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    mailClient.Authenticate(_mailSettings.SenderEmail,_mailSettings.SenderPassword);
                    mailClient.Send(emailMessage);
                    mailClient.Disconnect(true);
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}