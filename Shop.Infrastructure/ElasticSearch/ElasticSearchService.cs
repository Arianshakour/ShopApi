using Elastic.Clients.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.ElasticSearch
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly ElasticsearchClient _client;

        public ElasticSearchService(ElasticsearchClient client)
        {
            _client = client;
        }

        public void Index<T>(T document, string indexName)
            where T : class
        {
            //inja index misazim
            var response = _client.Index(
                document,
                x => x.Index(indexName));

            if (!response.IsValidResponse)
                throw new Exception(response.DebugInformation);
        }

        public void Update<T>(T document, string indexName)
            where T : class
        {
            //inja index misazim
            //age bashe replace mishe age na create khodesh handle mikone
            var response = _client.Index(
                document,
                x => x.Index(indexName));

            if (!response.IsValidResponse)
                throw new Exception(response.DebugInformation);
        }

        public void Delete(string indexName, string id)
        {
            //hazf index
            var response = _client.Delete(
                indexName,
                id);

            if (!response.IsValidResponse)
                throw new Exception(response.DebugInformation);
        }
    }
}
