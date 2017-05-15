using System;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace MockResponse.Site.ApiClient
{
    public class Client : IRestClient
    {
        public async Task<TResponse> GetAsync<TRequest, TResponse>(TRequest request)
            where TRequest : RequestBase
            where TResponse : class
        {
            var client = new HttpClient{BaseAddress = new Uri("http://localhost:1234")};
            var response = await client.GetStreamAsync(request.Path);
            var serializer = new DataContractJsonSerializer(typeof(TResponse));
            return serializer.ReadObject(response) as TResponse;
        }
    }

    public class RequestBase
    {
        public string Path { get; }
    }

    public interface IRestClient
    {
        Task<TResponse> GetAsync<TRequest, TResponse>(TRequest request)
            where TRequest : RequestBase
            where TResponse : class;
    }
}
