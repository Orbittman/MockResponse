using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using MockResponse.Api.Queries;
using MockResponse.Api.Queries.Parameters;

namespace MockResponse.Api.Filters
{
    public class AuthorisationFilterAttribute : ActionFilterAttribute
    {
        readonly IRequestContext _requestContext;
        readonly IAccountQuery _accountQuery;

        public AuthorisationFilterAttribute (IRequestContext requestContext, IAccountQuery accountQuery)
        {
            _requestContext = requestContext;
            _accountQuery = accountQuery;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(string.IsNullOrEmpty(_requestContext?.ApiKey)){
                context.Result = new UnauthorizedResult();
                return;
            }

            var account = _accountQuery.Execute(new AccountParmeters { ApiKey = _requestContext.ApiKey });
            if (account == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            _requestContext.PrimaryIdentity = account.PrimaryIdentity;
            _requestContext.AccountId = account.Id.ToString();
            base.OnActionExecuting(context);
        }
    }
}
