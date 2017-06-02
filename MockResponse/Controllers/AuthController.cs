using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MockResponse.Core.Utilities;
using MockResponse.Site.Infrastructure;
using MockResponse.Site.Models;
using MongoDB.Driver;

namespace MockResponse.Site.Controllers
{
    public class AuthController : BaseController
    {
        readonly INoSqlClient _dbClient;
		readonly IDateTimeProvider _dateTimeProvider;
		readonly IEmailClient _emailClient;

        public AuthController(
            INoSqlClient dbClient,
            ISiteRequestContext requestContext,
			IDateTimeProvider dateTimeProvider,
			IEmailClient emailClient)
            : base(requestContext)
        {
            _dbClient = dbClient;
            _dateTimeProvider = dateTimeProvider;
            _emailClient = emailClient;
        }

        public IActionResult Logout()
        {
            RequestContext.ClearSession();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            var session = RequestContext.Session;
            return View();
        }

		[HttpPost]
		public IActionResult LoginRequest(string authIdentity)
		{
			if (string.IsNullOrEmpty(authIdentity))
			{
				return RedirectToAction("Index", "Home");
			}

			var token = Guid.NewGuid().ToString();
			_dbClient.InsertOne(
				new LoginRequest { AuthIdentity = authIdentity, TimeStamp = _dateTimeProvider.Now, Token = token },
				nameof(Core.Data.Models.LoginRequest));

			// Email the link
			var loginMethod = ParseLoginMethod(authIdentity);
            var success = false;
			if (loginMethod == LoginMethod.Email)
			{
				success = _emailClient.Send(authIdentity, "tim@vouchercloud.com", $"<a href=\"http://127.0.0.1:1236/login/{token}\">Login</a>");
			}
			else
			{
				// success = [send an SMS]
			}

            return View("Login", new LoginRequestModel { AuthIdentity = authIdentity, Success = success });
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
                account = new Account { PrimaryIdentity = loginRequest.AuthIdentity, ApiKeys = new[] { Guid.NewGuid().ToString() } };
				_dbClient.InsertOne(
					account,
					nameof(Account));
			}

			RequestContext.SaveUserSession(
				new UserSession
				{
					Authenticated = true,
					AuthenticatedAt = _dateTimeProvider.Now,
					AuthenticationToken = token,
					Identity = account.PrimaryIdentity,
					Name = account.PrimaryIdentity
				});

			return Redirect("/");
		}
	}

	internal enum LoginMethod
	{
		Phone,
		Email
    }
}