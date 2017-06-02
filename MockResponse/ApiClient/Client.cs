using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MockResponse.Site.Configuration;
using MockResponse.Site.Models;

namespace MockResponse.Site.ApiClient
{
    public class Client : IRestClient
    {
        readonly AppConfig _configuration;
        readonly ISiteRequestContext _requestContext;

        public Client(IOptions<AppConfig> configuration, ISiteRequestContext requestContext)
        {
            _requestContext = requestContext;
            _configuration = configuration.Value;
        }

        public Task<TResponse> GetAsync<TRequest, TResponse>(TRequest request)
            where TRequest : RequestBase
            where TResponse : class
        {
            return Task.Run(() =>
            {
                using (var handler = new HttpClientHandler())
                {
                    using (var client = new HttpClient(handler) { BaseAddress = new Uri(_configuration.ApiAddress) })
                    {
                        var path = request.Path;
                        var properties = request.GetType().GetProperties();
                        foreach (var property in properties)
                        {
                            path = path.Replace($"{{property.Name}}", property.GetValue(request).ToString());
                        }

                        client.DefaultRequestHeaders.Add("x-apikey", _requestContext.ApiKey);
                        var response = client.GetAsync(path).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            return default(TResponse);
                        }

                        var serializer = new DataContractJsonSerializer(typeof(TResponse));
                        try
                        {
                            var result = serializer.ReadObject(response.Content.ReadAsStreamAsync().Result);
                            return result as TResponse;
                        }
                        catch
                        {
                            return default(TResponse);
                        }
                    }
                }
            });
        }
    }

    public interface IRestClient
    {
        Task<TResponse> GetAsync<TRequest, TResponse>(TRequest request)
            where TRequest : RequestBase
            where TResponse : class;
    }
}
