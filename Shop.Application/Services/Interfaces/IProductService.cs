using Shop.Domain.Dtoes.Category;
using Shop.Domain.Dtoes.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services.Interfaces
{
    public interface IProductService
    {
        List<CategoryDto> GetCategories();
        List<ProductDto> GetProductsByCategoryId(int categoryId);
    }
}
