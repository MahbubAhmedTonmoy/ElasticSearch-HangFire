using ElasticSearch.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearch.Utility
{
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services,
            IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                 .DefaultMappingFor<Product>
                 (m => m
                    .Ignore(p => p.Price)
                    .Ignore(p => p.Quantity)
                    .Ignore(p => p.Rating)
                );

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
            CreateIndex(client, defaultIndex);
        }
        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<Product>(x => x.AutoMap())
            );
        }
    }
}
