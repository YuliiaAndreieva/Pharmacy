namespace WepPha2.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? ExceptionDetails { get; set; }
        public int? StatusCode { get; set; }
        public string? StatusMessage { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public bool ShowExceptionDetails => !string.IsNullOrEmpty(ExceptionDetails);
        public bool ShowExceptionMessage => !string.IsNullOrEmpty(ExceptionMessage);
    }
}