using System.Collections.Generic;

using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MongoDB.Driver;

namespace MockResponse.Api.Queries
{
    public abstract class BaseQuery<TParameters, TResponse>
	{
		readonly INoSqlClient _dbClient;


		protected BaseQuery(INoSqlClient dbClient)
		{
			_dbClient = dbClient;
		}

		protected IEnumerable<TResponse> BaseExecute(TParameters request)
		{
			var filter = DefineFilter(request);
		    filter = ProtectedExecute(request, filter);

            var pageable = request as IPageable;
		    return pageable != null
		            ? _dbClient.Page(filter, typeof(TResponse).Name, pageable.Page, pageable.PageSize)
		            : _dbClient.Find(filter, typeof(TResponse).Name);
		}

	    protected virtual FilterDefinition<TResponse> ProtectedExecute(TParameters request, FilterDefinition<TResponse> filter)
	    {
	        return filter;
	    }

	    protected abstract FilterDefinition<TResponse> DefineFilter(TParameters request);
	}
}
