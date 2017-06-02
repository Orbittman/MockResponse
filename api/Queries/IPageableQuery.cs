using System.Collections.Generic;

namespace MockResponse.Api.Queries
{
    public interface IPageableQuery<TRequest, TResponse>
    {
		IEnumerable<TResponse> Execute(TRequest request);
	}
}