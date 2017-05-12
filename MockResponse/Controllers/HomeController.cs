using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MockResponse.Core.Utilities;

using MongoDB.Driver;

namespace MockResponse.Site.Controllers
{
    public class HomeController : BaseController
    {
        private readonly INoSqlClient _dbClient;
        private readonly IEmailClient _emailClient;
        private readonly ISiteRequestContext _requestContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public HomeController(
            INoSqlClient dbClient, 
            IEmailClient emailClient, 
            ISiteRequestContext requestContext,
            IDateTimeProvider dateTimeProvider)
            : base(requestContext)
        {
            _dbClient = dbClient;
            _emailClient = emailClient;
            _requestContext = requestContext;
            _dateTimeProvider = dateTimeProvider;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = CreateViewModel<HomeIndexModel>();
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult LoginRequest(string authIdentity)
        {
            if (string.IsNullOrEmpty(authIdentity))
            {
                return RedirectToAction("Index");
            }

            var token = Guid.NewGuid().ToString();
            _dbClient.InsertOne(new LoginRequest{ AuthIdentity = authIdentity, TimeStamp = _dateTimeProvider.Now, Token = token}, nameof(Core.Data.Models.LoginRequest));

            // Email the link
            var loginMethod = ParseLoginMethod(authIdentity);
            if (loginMethod == LoginMethod.Email)
            {
                _emailClient.Send("tim@vouchercloud.com", "tim@vouchercloud.com", $"<a href=\"http://127.0.0.1:1236/login/{token}\">Login</a>");
            }
            else
            {
                // send an SMS
            }

            return View("Login");
        }

        private LoginMethod ParseLoginMethod(string input)
        {
            return long.TryParse(input.Replace(" ", string.Empty), out long phoneNumber) ? LoginMethod.Phone : LoginMethod.Email;
        }

        [HttpGet]
        public IActionResult Login(string token)
        {
            var filter = Builders<LoginRequest>.Filter.Eq(r => r.Token, token);
            var loginRequest = _dbClient.Find(filter, nameof(Core.Data.Models.LoginRequest)).FirstOrDefault();
            if (loginRequest == null || loginRequest.TimeStamp < _dateTimeProvider.Now.AddMinutes(-5))
            {
                return Unauthorized();
            }

            _requestContext.SaveUserSession(new UserSession
            {
                Authenticated = true,
                AuthenticatedAt = _dateTimeProvider.Now,
                AuthenticationToken = token
            });

            return Redirect("/");
        }
    }

    public class BaseController : Controller
    {
        private readonly ISiteRequestContext _requestContext;

        public BaseController(ISiteRequestContext requestContext)
        {
            _requestContext = requestContext;
        }
        protected TModel CreateViewModel<TModel>(Action<TModel> constructionPredicate = null)
            where TModel : BaseViewModel, new()
        {
            var model = new TModel { Authenticated = _requestContext.Authenticated };
            constructionPredicate?.Invoke(model);
            return model;
        }
    }

    public class HomeIndexModel : BaseViewModel
    {
    }

    public abstract class BaseViewModel
    {
        public bool Authenticated { get; set; }
    }

    internal enum LoginMethod
    {
        Phone,
        Email
    }

    public interface IEmailClient
    {
        bool Send(string to, string from, string body);
    }

    public class EmailClient : IEmailClient
    {
        public bool Send(string to, string @from, string body)
        {
            return true;
        }
    }
}
