using System;

using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MockResponse.Core.Requests;

namespace MockResponse.Core.Commands
{
    public class AccountCommand : IAccountCommand
    {
        private readonly INoSqlClient _dbClient;

        public AccountCommand(
            INoSqlClient dbClient)
        {
            _dbClient = dbClient;
        }

        public void Execute(SaveAccountRequest request)
        {
            var account = new Account { PrimaryIdentity = request.AuthIdentity, ApiKeys = new[] { Guid.NewGuid().ToString() } };
            _dbClient.InsertOne(
                account,
                nameof(Account));
        }
    }
}
