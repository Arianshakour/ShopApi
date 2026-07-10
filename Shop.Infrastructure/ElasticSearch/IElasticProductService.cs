using Shop.Domain.Dtoes.Product;
using Shop.Domain.Dtoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.ElasticSearch
{
    public interface IElasticProductService
    {
        //generic neveshtim ke badan khastam vase categori ham elastic konam rahat basham

        void IndexProduct(ProductDto product);
        void DeleteProduct(int productId);
        void UpdateProduct(ProductDto product);
        List<ProductDto> SearchProducts(FilteringDto filter);//baraye search hast
    }
}
