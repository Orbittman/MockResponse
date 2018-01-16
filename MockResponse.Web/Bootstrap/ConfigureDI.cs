using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MockResponse.Core.Caching;
using MockResponse.Core.Data;
using MockResponse.Core.Utilities;
using MockResponse.Web.ApiClient;
using MockResponse.Web.Configuration;
using MockResponse.Web.Infrastructure;
using MockResponse.Web.Models;
using ServiceStack.Redis;

namespace MockResponse.Web.Bootstrap
{
    public static class ConfigureDI
    {
        public static void Configure(IServiceCollection services, AppConfig config)
        {
            services.AddSingleton<INoSqlClient>(client => new MongoDbClient($"mongodb://{config.MongoUsername}:{config.MongoPassword}@cluster0-shard-00-00-zlhjf.mongodb.net:27017,cluster0-shard-00-01-zlhjf.mongodb.net:27017,cluster0-shard-00-02-zlhjf.mongodb.net:27017/IDL_Monitor?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin"));
            services.AddTransient(c => new RedisManagerPool("localhost:6379").GetClient());
            services.AddSingleton<ICacheClient, RedisCacheClient>();
            services.AddSingleton<IRestClient, Client>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IDomainContext>((arg) => new DomainContext
            {
                Configuration = config
            });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IEmailClient, EmailClient>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<ISiteRequestContext, SiteRequestContext>();
        }
    }
}
