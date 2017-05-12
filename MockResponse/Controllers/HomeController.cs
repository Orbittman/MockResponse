using System;
using System.Linq;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;

using MongoDB.Driver;

namespace MockResponse.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly INoSqlClient _dbClient;
        private readonly IEmailClient _emailClient;

        public HomeController(INoSqlClient dbClient, IEmailClient emailClient)
        {
            _dbClient = dbClient;
            _emailClient = emailClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult LoginRequest(string emailAddress)
        {
            // Create a link for the user to log in against
            //var filter = Builders<LoginRequest>.Filter.Eq(r => r.Email, emailAddress);
            //var response = _dbClient.Find(filter, "Responses").FirstOrDefault();

            var token = Guid.NewGuid().ToString();
            _dbClient.InsertOne(new LoginRequest{Email = emailAddress, TimeStamp = DateTime.UtcNow, Token = token}, nameof(Core.Data.Models.LoginRequest));

            // Email the link
            _emailClient.Send("tim@vouchercloud.com", "tim@vouchercloud.com", $"<a href=\"http://127.0.0.1:1236/login/{token}\">Login</a>");

            return View("Login");
        }

        [HttpGet]
        public IActionResult Login(string token)
        {
            var filter = Builders<LoginRequest>.Filter.Eq(r => r.Token, token);
            var loginRequest = _dbClient.Find(filter, nameof(Core.Data.Models.LoginRequest)).FirstOrDefault();
            if (loginRequest == null || loginRequest.TimeStamp < DateTime.UtcNow.AddMinutes(-5))
            {
                return Unauthorized();
            }

            var sessionToken = Guid.NewGuid().ToString();
            Response.Cookies.Append("mrauth", sessionToken);

            return Redirect("/");
        }
    }

    public interface IEmailClient
    {
        bool Send(string to, string from, string body);
    }

    class EmailClient : IEmailClient
    {
        public bool Send(string to, string @from, string body)
        {
            return true;
        }
    }
}
