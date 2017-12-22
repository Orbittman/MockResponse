using Microsoft.AspNetCore.Mvc;

using MockResponse.Core.Data;
using MockResponse.Core.Utilities;
using MockResponse.Web.Models;

namespace MockResponse.Web.Controllers
{
    public class HomeController : BaseController
    {
        readonly INoSqlClient _dbClient;
        readonly IDateTimeProvider _dateTimeProvider;

        public HomeController(
            INoSqlClient dbClient,
            ISiteRequestContext requestContext,
            IDateTimeProvider dateTimeProvider,
            IDomainContext domainContext)
            : base(requestContext, domainContext)
        {
            _dbClient = dbClient;
            _dateTimeProvider = dateTimeProvider;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = CreateViewModel<HomeIndexModel>();
            return View("Index", model);
        }
    }
}
