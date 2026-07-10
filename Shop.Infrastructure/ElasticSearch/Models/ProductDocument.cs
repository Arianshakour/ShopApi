using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.ElasticSearch.Models
{
    //in class baraye ine ke ma dto ra dar Elasticsearch zakhire nemikonim
    //alan to in proje shayad shabi ham bashan vali aslesh zakhire nemikonim
    //az class zir estefade mikonim
    public class ProductDocument
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public int CategoryId { get; set; }
    }
}
