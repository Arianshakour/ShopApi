using Shop.Domain.Dtoes;
using Shop.Domain.Dtoes.Category;
using Shop.Domain.Dtoes.Product;
using Shop.Domain.Entities;
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
        CategoryDto GetCategory(int categoryId);
        CategoryDto GetCategory2(int categoryId, bool includeProducts);
        CategoryDto AddCategory(CategoryForCreateDto category);
        bool UpdateCategory(int id, CategoryForEditDto category);
        bool DeleteCategory(int id);
        bool IsExistCategory(int id);

        //Product
        List<ProductDto> GetProductsByCategoryId(int categoryId);
        List<ProductDto> GetAllProducts(FilteringDto filter);
        ProductDto GetProductByCategoryId(int categoryId, int id);
        ProductDto GetProduct(int id);
        ProductDto AddProduct(int categoryId, ProductForCreateDto create);
        bool UpdateProduct(int categoryId, int productId, ProductForEditDto edit);
        bool DeleteProduct(int productId);
        bool IsExistProduct(int id);
    }
}
