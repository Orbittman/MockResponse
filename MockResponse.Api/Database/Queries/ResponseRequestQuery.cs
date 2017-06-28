using System.Collections.Generic;
using System.Linq;

using MockResponse.Api.Database.MongoDb;
using MockResponse.Core.Data;

using ResponseRequest = MockResponse.Core.Requests.ResponseRequest;

namespace MockResponse.Api.Database.Queries
{
    public class ResponseRequestQuery : IResponseRequestQuery
    {
        private INoSqlClient _client;

        public ResponseRequestQuery(INoSqlClient client)
        {
            _client = client;
        }

        public IEnumerable<HttpResponse> Execute(ResponseRequest request)
        {
            return Enumerable.Empty<HttpResponse>();
        }
    }
}
