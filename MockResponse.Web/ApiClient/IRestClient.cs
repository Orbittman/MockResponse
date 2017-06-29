using System.Threading.Tasks;

using MockResponse.Core.Requests;

namespace MockResponse.Web.ApiClient
{
    public interface IRestClient
    {
        Task<TResponse> GetAsync<TRequest, TResponse>(TRequest request)
            where TRequest : RequestBase
            where TResponse : class;

        Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request)
            where TRequest : RequestBase
            where TResponse : class;
    }
}