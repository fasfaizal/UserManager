using System.Net;

namespace UserManager.Common.Exceptions
{
    public class ApiValidationException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public ApiValidationException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
