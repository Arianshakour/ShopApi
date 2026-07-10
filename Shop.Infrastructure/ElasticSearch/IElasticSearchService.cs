using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.ElasticSearch
{
    public interface IElasticSearchService
    {
        //generic neveshtim ke badan khastam vase categori ham elastic konam rahat basham
        //search omomi nist pas generic neminevisim dar interface har qesmat search ra minevisim
        void Index<T>(T document, string indexName) where T : class;

        void Update<T>(T document, string indexName) where T : class;

        void Delete(string indexName, string id);
    }
}
