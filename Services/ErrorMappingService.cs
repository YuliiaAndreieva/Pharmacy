using WepPha2.Constants;
using WepPha2.Models;

namespace WepPha2.Services
{
    public interface IErrorMappingService
    {
        ErrorInfo GetErrorInfo(int statusCode, string path);
    }

    public class ErrorMappingService : IErrorMappingService
    {
        private readonly ILogger<ErrorMappingService> _logger;

        public ErrorMappingService(ILogger<ErrorMappingService> logger)
        {
            _logger = logger;
        }

        public ErrorInfo GetErrorInfo(int statusCode, string path)
        {
            var errorInfo = statusCode switch
            {
                404 => ErrorMessages.PageNotFound,
                403 => ErrorMessages.AccessForbidden,
                401 => ErrorMessages.Unauthorized,
                500 => ErrorMessages.InternalServerError,
                _ => ErrorMessages.GeneralError
            };

            Log(statusCode, path, errorInfo.LogLevel);

            return errorInfo;
        }

        private void Log(int statusCode, string path, LogLevel level)
        {
            var message = $"{statusCode} error occurred for path: {path}";
            _logger.Log(level, message);
        }
    }
} 