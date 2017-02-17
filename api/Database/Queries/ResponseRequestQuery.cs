using System.Collections.Generic;
using System.Linq;

public class ResponseRequestQuery : IResponseRequestQuery {
    private INoSqlClient _client;

    public ResponseRequestQuery(INoSqlClient client){
        _client = client;
    }

    public IEnumerable<HttpResponse> Execute(ResponseRequest request){
        return Enumerable.Empty<HttpResponse>();
    }
}