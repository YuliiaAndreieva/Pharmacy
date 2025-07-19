using Microsoft.AspNetCore.Mvc;
using WepPha2.Models;
using WepPha2.Services;
using WepPha2.Constants;

namespace WepPha2.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;
        private readonly IErrorMappingService _errorMappingService;

        public ErrorController(ILogger<ErrorController> logger, IErrorMappingService errorMappingService)
        {
            _logger = logger;
            _errorMappingService = errorMappingService;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var errorInfo = _errorMappingService.GetErrorInfo(statusCode, HttpContext.Request.Path);
            
            var errorViewModel = new ErrorViewModel
            {
                StatusCode = statusCode,
                RequestId = HttpContext.TraceIdentifier,
                StatusMessage = errorInfo.StatusMessage,
                ExceptionMessage = errorInfo.ExceptionMessage
            };

            return View("Error", errorViewModel);
        }

        [Route("Error")]
        public IActionResult Error()
        {
            _logger.LogError("General error occurred for path: {Path}", HttpContext.Request.Path);
            
            var errorViewModel = new ErrorViewModel
            {
                RequestId = HttpContext.TraceIdentifier,
                StatusCode = 500,
                StatusMessage = ErrorMessages.UnexpectedError.StatusMessage,
                ExceptionMessage = ErrorMessages.UnexpectedError.ExceptionMessage
            };

            return View("Error", errorViewModel);
        }
    }
} 