using System;
using MockResponse.Core.Requests;

namespace MockResponse.Core.Commands
{
    public class AccountCommand : IAccountCommand
    {
        public void Execute(SaveAccountRequest request)
        {
            account = new Account { PrimaryIdentity = request.AuthIdentity, ApiKeys = new[] { Guid.NewGuid().ToString() } };
            _dbClient.InsertOne(
                account,
                nameof(Account));
        }
    }
}
