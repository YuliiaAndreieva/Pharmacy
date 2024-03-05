using Microsoft.Extensions.Options;
using Vonage;
using Vonage.Request;
using WepPha2.Models;

namespace WepPha2.Services.SmsSending;

public class SmsService
{
    private readonly SmsSettings _smsSettings;
    
    public SmsService(IOptions<SmsSettings> mailSettingsOptions)
    {
        _smsSettings = mailSettingsOptions.Value;
    }
    
    public async void SendSms(
        SmsData smsData)
    {
        var credentials = Credentials.FromApiKeyAndSecret(
            _smsSettings.ApiKey,
            _smsSettings.ApiSecret
        );

        var vonageClient = new VonageClient(credentials);
        
        var res= await vonageClient.SmsClient.SendAnSmsAsync(new Vonage.Messaging.SendSmsRequest()
        {
            To = smsData.To,
            From = smsData.From,
            Text = smsData.SmsBody
        });
    }


}