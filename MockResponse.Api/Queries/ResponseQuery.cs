using System.Linq;

using MockResponse.Api.Queries.Parameters;
using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;

using MongoDB.Bson;
using MongoDB.Driver;

namespace MockResponse.Api.Queries
{
    public class ResponseQuery : ApiKeyedQuery<ResponseParameters, Response>, IResponseQuery
    {
        public ResponseQuery(IRequestContext requestContext, INoSqlClient dbClient)
            : base(requestContext, dbClient)
        {
        }

        protected override FilterDefinition<Response> DefineFilter(ResponseParameters request)
        {
            return Builders<Response>.Filter.Eq(r => r.Id, new ObjectId(request.ResponseId));
        }

        public Response Execute(ResponseParameters request)
        {
            var result = BaseExecute(request);
            return result.SingleOrDefault();
        }
    }
}