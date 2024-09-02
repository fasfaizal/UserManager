using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UserManager.Common.Exceptions;

namespace UserManager.API.Controllers
{
    [ApiController]
    public class GlobalErrorHandlerController : ControllerBase
    {
        ILogger<GlobalErrorHandlerController> _logger;
        public GlobalErrorHandlerController(ILogger<GlobalErrorHandlerController> logger)
        {
            _logger = logger;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/handle-error")]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (exception != null)
            {
                // Log error and send custom response for ApiValidationException
                _logger.LogError(exception, $"Error: {exception.Message}");
                if (exception is ApiValidationException)
                {
                    var apiEx = exception as ApiValidationException;
                    return Problem(detail: apiEx?.Message, statusCode: (int)apiEx.StatusCode);
                }
                else
                {
                    return Problem(detail: exception.Message);
                }
            }
            return Ok();
        }
    }
}
