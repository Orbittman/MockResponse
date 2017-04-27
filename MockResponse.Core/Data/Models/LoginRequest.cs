using System;

namespace MockResponse.Core.Data.Models
{
    public class LoginRequest
    {
        public string Email { get; set; }

        public Guid Token { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
