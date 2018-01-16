using System;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

using MockResponse.Core.Requests;
using MockResponse.Web.Configuration;
using MockResponse.Web.Models;

using ServiceStack;

namespace MockResponse.Web.ApiClient
{
    public class Client : IRestClient
    {
        readonly AppConfig _configuration;
        readonly ISiteRequestContext _requestContext;

        public Client(AppConfig configuration, ISiteRequestContext requestContext)
        {
            _requestContext = requestContext;
            _configuration = configuration;
        }

        public Task<TResponse> GetAsync<TRequest, TResponse>(TRequest request)
            where TRequest : RequestBase
            where TResponse : class
        {
            return CallClientAsync<TRequest, TResponse>(request, (client, path) => client.GetAsync(path));
        }

        public Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request)
            where TRequest : RequestBase
            where TResponse : class
        {
            return CallClientAsync<TRequest, TResponse>(request, (client, path) => client.PostAsync(path, new StringContent(request.ToJson(), Encoding.UTF8,
                                                                                                                            "application/json")));
        }

        private Task<TResponse> CallClientAsync<TRequest, TResponse>(TRequest request, Func<HttpClient, string, Task<HttpResponseMessage>> clientFunction)
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
                                            path = path.Replace($"{{{property.Name}}}", property.GetValue(request).ToString());
                                        }

                                        client.DefaultRequestHeaders.Add("x-apikey", _requestContext.ApiKey);

                                        var response = clientFunction(client, path).Result;
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
}
