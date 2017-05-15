using Microsoft.AspNetCore.Mvc;

using MockResponse.Core.Data;
using MockResponse.Core.Utilities;
using MockResponse.Site.Models;

namespace MockResponse.Site.Controllers
{
    public class AuthController : BaseController
    {
        private readonly INoSqlClient _dbClient;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AuthController(
            INoSqlClient dbClient,
            ISiteRequestContext requestContext,
            IDateTimeProvider dateTimeProvider)
            : base(requestContext)
        {
            _dbClient = dbClient;
            _dateTimeProvider = dateTimeProvider;
        }

        public IActionResult Index()
        {
            var session = RequestContext.Session;
            return View();
        }
    }

}