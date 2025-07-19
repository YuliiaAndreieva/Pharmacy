using WepPha2.Models;

namespace WepPha2.Constants
{
    public static class ErrorMessages
    {
        public static readonly ErrorInfo PageNotFound = new("Page Not Found", "The page you are looking for could not be found.", LogLevel.Warning);
        public static readonly ErrorInfo AccessForbidden = new("Access Forbidden", "You don't have permission to access this resource.", LogLevel.Warning);
        public static readonly ErrorInfo Unauthorized = new("Unauthorized", "Please log in to access this resource.", LogLevel.Warning);
        public static readonly ErrorInfo InternalServerError = new("Internal Server Error", "An internal server error occurred.", LogLevel.Error);
        public static readonly ErrorInfo GeneralError = new("Error", "An error occurred while processing your request.", LogLevel.Warning);
        public static readonly ErrorInfo UnexpectedError = new("Internal Server Error", "An unexpected error occurred.", LogLevel.Error);
    }
} 