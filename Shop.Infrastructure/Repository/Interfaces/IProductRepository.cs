using Shop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.Repository.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetProducts();
        List<Product> GetProductsByCategoryId(int categoryId);
        Product GetProduct(int id);
        Product GetProductByCategoryId(int categoryId,int productId);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        void Save();

        //Category
        List<Category> GetCategories();
        bool IsExistCategory(int id);
        Category GetCategory(int id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }
}
