using System;

using Microsoft.AspNetCore.Mvc;

using MockResponse.Web.Models;

namespace MockResponse.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected BaseController(ISiteRequestContext requestContext, IDomainContext domainContext)
        {
            RequestContext = requestContext;
            DomainContext = domainContext;
        }

        protected ISiteRequestContext RequestContext { get; }

        protected IDomainContext DomainContext { get; }

        protected TModel CreateViewModel<TModel>(Action<TModel> constructionPredicate = null)
            where TModel : BaseViewModel, new()
        {
            var model = new TModel { 
                Authenticated = RequestContext.Authenticated, 
                RequestContext = RequestContext,
                Url = Url,
                ClientStyles = DomainContext.ClientStyles,
                ClientJs = DomainContext.ClientJs
            };

            constructionPredicate?.Invoke(model);
            return model;
        }
    }
}