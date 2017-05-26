﻿using System.Collections.Generic;
using System.Linq;

using MockResponse.Api.Queries.Parameters;
using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;

using MongoDB.Driver;

namespace MockResponse.Api.Queries
{
    public class AccountQuery : BaseQuery<AccountParmeters, Account>, IAccountQuery
    {
        public AccountQuery(INoSqlClient dbClient)
            : base(dbClient)
        {
        }

        protected override FilterDefinition<Account> DefineFilter(AccountParmeters request)
        {
            return Builders<Account>.Filter.AnyEq(f => f.ApiKeys, request.ApiKey);
        }

        protected override Account BuildResponse(IEnumerable<Account> dbResponse)
        {
            return dbResponse.SingleOrDefault();
        }
    }
}
