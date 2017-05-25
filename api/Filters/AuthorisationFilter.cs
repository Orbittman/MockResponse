using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MockResponse.Api.Filters
{
    public class AuthorisationFilterAttribute : ActionFilterAttribute
    {
        readonly IRequestContext _requestContext;

        public AuthorisationFilterAttribute (IRequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(string.IsNullOrEmpty(_requestContext?.ApiKey)){
                context.Result = new UnauthorizedResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
