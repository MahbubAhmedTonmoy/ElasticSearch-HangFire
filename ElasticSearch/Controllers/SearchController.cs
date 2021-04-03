using ElasticSearch.Model;
using ElasticSearch.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IElasticSearchService _productService;
        private readonly IElasticClient _elasticClient;
        private readonly ILogger _logger;

        public SearchController(IElasticSearchService productService, IElasticClient elasticClient)
        {
            _productService = productService;
            _elasticClient = elasticClient;
        }
        /// <summary>
        /// </summary>
        /// <param name="queryText">Apache Lucene query, e.g. SAP AND (FI-CO OR “fi co” OR finance) AND (consult* OR archit*) NOT director</param>
        /// <returns>Candidates id</returns>
        [HttpGet("find")]
        public async Task<IActionResult> Find(string query, int page = 1, int pageSize = 50)
        {
            var response = await _elasticClient.SearchAsync<Product>(
                 s => s.Query(q => q.QueryString(d => d.Query('*' + query + '*')))
                     .From((page - 1) * pageSize)
                     .Size(pageSize));

            //var response = await _elasticClient.SearchAsync<Product>(
            //     s => s.Query(q => q.MatchAll())
            //         .From((page - 1) * pageSize)
            //         .Size(pageSize));
            
            if (!response.IsValid)
            {
                // We could handle errors here by checking response.OriginalException 
                //or response.ServerError properties
                _logger.LogError("Failed to search documents");
                return Ok(new Product[] { });
            }

            return Ok(response.Documents);
        }
    }
}
