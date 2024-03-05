using System.Text.Json.Serialization;

namespace WepPha2.Services.SmsSending;

public class ResponseDto
{
    [JsonPropertyName("requestId")]
    public string RequestId { get; set; }
}
