using Microsoft.AspNetCore.Mvc.Filters;
using MockResponse.Core.Logging;

namespace MockResponse.Api
{
	public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
		private readonly ILogger _logger;

		public ErrorHandlingFilter(ILogger logger){
			_logger = logger;
		}

		public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
			_logger.Trace(exception.Message);

            context.ExceptionHandled = true; //optional 
        }
	}
}