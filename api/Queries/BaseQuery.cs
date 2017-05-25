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

			if (request is IKeyedEntity)
			{
				filter &= Builders<TResponse>.Filter.Eq(f => ((IKeyedEntity)f).ApiKey, _requestContext.ApiKey);
			}

		    var pageable = request as IPageable;
		    return BuildResponse(
		        pageable != null
		            ? _dbClient.Page(filter, typeof(TResponse).Name, pageable.Page, pageable.PageSize)
		            : _dbClient.Find(filter, typeof(TResponse).Name));
		}

		protected abstract FilterDefinition<TResponse> DefineFilter(TParameters request);

		protected abstract TResponse BuildResponse(IEnumerable<TResponse> dbResponse);
	}
}
