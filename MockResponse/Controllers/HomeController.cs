using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MockResponse.Core.Utilities;
using MockResponse.Site.Models;

using MongoDB.Driver;

namespace MockResponse.Site.Controllers
{
    public class HomeController : BaseController
    {
        private readonly INoSqlClient _dbClient;
        private readonly IEmailClient _emailClient;
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
            _dbClient.InsertOne(
                new LoginRequest { AuthIdentity = authIdentity, TimeStamp = _dateTimeProvider.Now, Token = token },
                nameof(Core.Data.Models.LoginRequest));

            // Email the link
            var loginMethod = ParseLoginMethod(authIdentity);
            if (loginMethod == LoginMethod.Email)
            {
                _emailClient.Send(authIdentity, "tim@vouchercloud.com", $"<a href=\"http://127.0.0.1:1236/login/{token}\">Login</a>");
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

            // Find an existing account record
            var account = _dbClient.Find(Builders<Account>.Filter.Eq(r => r.PrimaryIdentity, loginRequest.AuthIdentity), nameof(Account))
                                   .FirstOrDefault();
            if (account == null)
            {
                _dbClient.InsertOne(new Account { PrimaryIdentity = loginRequest.AuthIdentity, ApiKey = Guid.NewGuid().ToString() }, nameof(Account));
            }

            RequestContext.SaveUserSession(
                new UserSession
                {
                    Authenticated = true,
                    AuthenticatedAt = _dateTimeProvider.Now,
                    AuthenticationToken = token
                });

            return Redirect("/");
        }
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
