using System;
using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MockResponse.Api.Commands
{
    public class DeleteResponseCommand : IResponseDeleteCommand
    {
        readonly IRequestContext _requestContext;
        readonly INoSqlClient _dbClient;

        public DeleteResponseCommand(IRequestContext requestContext, INoSqlClient dbClient)
        {
            _dbClient = dbClient;
            _requestContext = requestContext;
        }

        public long Execute(ResponseDeleteParameters request)
        {
			// Wrap all this in a command object
			var filter = Builders<Response>.Filter
										   .Eq(r => r.Id, new ObjectId(request.ResponseId))
										   & Builders<Response>.Filter
										   .Eq(r => r.Account, new ObjectId(_requestContext.AccountId));

			return _dbClient.DeleteOne(filter, nameof(Response));
		}
    }
}
