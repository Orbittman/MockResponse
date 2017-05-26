using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;

using MongoDB.Bson;
using MongoDB.Driver;

namespace MockResponse.Api.Queries
{
    public abstract class ApiKeyedQuery<TParameters, TResponse>
        : BaseQuery<TParameters, TResponse> 
        where TResponse : IAccountEntity
    {
        private readonly IRequestContext _requestContext;

        protected ApiKeyedQuery(IRequestContext requestContext, INoSqlClient dbClient)
            : base(dbClient)
        {
            _requestContext = requestContext;
        }

        public override FilterDefinition<TResponse> ProtectedExecute(TParameters request, FilterDefinition<TResponse> filter)
        {
            filter &= Builders<TResponse>.Filter.Eq(f => f.Account, new ObjectId(_requestContext.AccountId));
            return base.ProtectedExecute(request, filter);
        }
    }
}
