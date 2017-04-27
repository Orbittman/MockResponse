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
        private readonly IMapper _mapper;
        private readonly INoSqlClient _dbClient;

        public HomeController(IMapper mapper, INoSqlClient dbClient)
        {
            _mapper = mapper;
            _dbClient = dbClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult Login(string emailAddress)
        {
            // Create a link for the user to log in against
            //var filter = Builders<LoginRequest>.Filter.Eq(r => r.Email, emailAddress);
            //var response = _dbClient.Find(filter, "Responses").FirstOrDefault();

            var token = Guid.NewGuid();
            _dbClient.InsertOne(new LoginRequest{Email = emailAddress, TimeStamp = DateTime.UtcNow, Token = token}, "Responses");

            // Email the link


            return View("Login");
        }
    }
}
