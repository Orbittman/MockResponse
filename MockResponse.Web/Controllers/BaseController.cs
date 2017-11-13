using System;

using Microsoft.AspNetCore.Mvc;

using MockResponse.Web.Models;

namespace MockResponse.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected BaseController(ISiteRequestContext requestContext)
        {
            RequestContext = requestContext;
        }

        protected ISiteRequestContext RequestContext { get; }

        protected TModel CreateViewModel<TModel>(Action<TModel> constructionPredicate = null)
            where TModel : BaseViewModel, new()
        {
            var model = new TModel { 
                Authenticated = RequestContext.Authenticated, 
                RequestContext = RequestContext,
                Url = Url
            };
            constructionPredicate?.Invoke(model);
            return model;
        }
    }
}