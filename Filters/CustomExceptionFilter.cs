using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WepPha2.Models;

namespace WepPha2.Filters
{
    public abstract class BaseExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        protected BaseExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public virtual void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Exception occurred in {Controller}.{Action}",
                context.RouteData.Values["controller"],
                context.RouteData.Values["action"]);

            var errorViewModel = CreateErrorViewModel(context);

            context.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ErrorViewModel>(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    Model = errorViewModel
                }
            };

            context.ExceptionHandled = true;
        }

        protected abstract ErrorViewModel CreateErrorViewModel(ExceptionContext context);
    }

    public class DevelopmentExceptionFilter : BaseExceptionFilter
    {
        public DevelopmentExceptionFilter(ILogger<DevelopmentExceptionFilter> logger) : base(logger)
        {
        }

        protected override ErrorViewModel CreateErrorViewModel(ExceptionContext context)
        {
            return new ErrorViewModel
            {
                RequestId = context.HttpContext.TraceIdentifier,
                ExceptionMessage = context.Exception.Message,
                ExceptionDetails = context.Exception.ToString()
            };
        }
    }

    public class ProductionExceptionFilter : BaseExceptionFilter
    {
        public ProductionExceptionFilter(ILogger<ProductionExceptionFilter> logger) : base(logger)
        {
        }

        protected override ErrorViewModel CreateErrorViewModel(ExceptionContext context)
        {
            return new ErrorViewModel
            {
                RequestId = context.HttpContext.TraceIdentifier,
                ExceptionMessage = "An error occurred.",
                ExceptionDetails = null
            };
        }
    }
} 