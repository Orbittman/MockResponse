using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using MockResponse.Core.Commands;
using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MockResponse.Core.Requests;
using MockResponse.Web.Infrastructure;
using MockResponse.Web.Models;

using MongoDB.Driver;

namespace MockResponse.Web.Controllers
{
    public class AuthController : BaseController
    {
        readonly INoSqlClient _dbClient;
		readonly IDateTimeProvider _dateTimeProvider;
		readonly IEmailClient _emailClient;
        readonly ISessionCommand _sessionCommand;
        readonly IAccountCommand _accountCommand;

        public AuthController(
            INoSqlClient dbClient,
            ISiteRequestContext requestContext,
			IDateTimeProvider dateTimeProvider,
			IEmailClient emailClient,
            IDomainContext domainContext,
            ISessionCommand sessionCommand,
            IAccountCommand accountCommand)
            : base(requestContext, domainContext)
        {
            _accountCommand = accountCommand;
            _dbClient = dbClient;
            _dateTimeProvider = dateTimeProvider;
            _emailClient = emailClient;
            _sessionCommand = sessionCommand;
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
				new LoginRequest { AuthIdentity = authIdentity, TimeStamp = _dateTimeProvider.UtcNow, Token = token },
				nameof(Core.Data.Models.LoginRequest));

			// Email the link
			var loginMethod = ParseLoginMethod(authIdentity);
            var success = false;
			if (loginMethod == LoginMethod.Email)
			{
			    var url = Url.Action("Login", "Auth", new { token });
				success = _emailClient.Send(authIdentity, "MockResponse login", string.Format("<a href=\"{0}\"> Login</a>", url));
			}
			else
			{
				// success = [send an SMS]
			}

            //return View("Login", new LoginRequestModel { AuthIdentity = authIdentity, Success = success });
            return Login(token);
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
            if (loginRequest == null || loginRequest.TimeStamp < _dateTimeProvider.UtcNow.AddMinutes(-5))
            {
                return Unauthorized();
            }

            // Find an existing account record
            var account = _dbClient.Find(Builders<Account>.Filter.Eq(r => r.PrimaryIdentity, loginRequest.AuthIdentity), nameof(Account))
                                   .FirstOrDefault();
            if (account == null)
            {
                _accountCommand.Execute(new SaveAccountRequest { AuthIdentity = loginRequest.AuthIdentity });
                account = new Account { PrimaryIdentity = loginRequest.AuthIdentity, ApiKeys = new[] { Guid.NewGuid().ToString() } };
                _dbClient.InsertOne(
                    account,
                    nameof(Account));
            }

            var (sessionKey, expiry) = RequestContext.SaveUserSession(
                new UserSession
                {
                    Authenticated = true,
                    AuthenticatedAt = _dateTimeProvider.UtcNow,
                    AuthenticationToken = token,
                    Identity = account.PrimaryIdentity,
                    Name = account.PrimaryIdentity
                });

            _sessionCommand.Execute(new SaveSessionRecordRequest
            {
                AccountId = account.Id,
                Expiry = expiry,
                SessionKey = sessionKey
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