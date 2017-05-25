using System.Collections.Generic;
using System.Reflection;

using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MongoDB.Driver;

namespace MockResponse.Api.Queries
{
    public abstract class BaseQuery<TParameters, TResponse> : IQuery<TParameters, TResponse>
	{
		readonly IRequestContext _requestContext;
		readonly INoSqlClient _dbClient;


		protected BaseQuery(IRequestContext requestContext, INoSqlClient dbClient)
		{
			_requestContext = requestContext;
			_dbClient = dbClient;
		}

		public TResponse Execute(TParameters request)
		{
			var filter = DefineFilter(request);

			if (typeof(IKeyedEntity).GetTypeInfo().IsAssignableFrom(typeof(TResponse).GetTypeInfo()))
			{
				filter &= Builders<TResponse>.Filter.Eq(f => ((IKeyedEntity)f).ApiKey, _requestContext.ApiKey);
			}

		    if (typeof(IPageable).GetTypeInfo().IsAssignableFrom(typeof(TResponse).GetTypeInfo()))
            {
                return BuildResponse(_dbClient.Page(filter, typeof(TResponse).Name, ((IPageable)request).Page, ((IPageable)request).PageSize));
            }

			return BuildResponse(_dbClient.Find(filter, typeof(TResponse).Name));
		}

		protected abstract FilterDefinition<TResponse> DefineFilter(TParameters request);

		protected abstract TResponse BuildResponse(IEnumerable<TResponse> dbResponse);
	}
}
