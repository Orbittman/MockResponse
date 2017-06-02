using System.Collections.Generic;

using MockResponse.Api.Queries.Parameters;
using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MockResponse.Api.Queries
{
    public class ResponsesQuery : ApiKeyedQuery<ResponseParameters, Response>, IResponseQuery
    {
        public ResponsesQuery(IRequestContext requestContext, INoSqlClient dbClient) : base(requestContext, dbClient)
        {
        }

        protected override FilterDefinition<Response> DefineFilter(ResponseParameters request)
        {
            return new BsonDocument();
        }

        IEnumerable<Response> IPageableQuery<ResponseParameters, Response>.Execute(ResponseParameters request)
        {
			var result = BaseExecute(request);
			return result;
        }
    }
}
