using System;

using Microsoft.AspNetCore.Mvc;

using MockResponse.Site.Models;

namespace MockResponse.Site.Controllers
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
            var model = new TModel { Authenticated = RequestContext.Authenticated, RequestContext = RequestContext };
            constructionPredicate?.Invoke(model);
            return model;
        }
    }
}