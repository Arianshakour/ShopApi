using Shop.Application.Services.Interfaces;
using Shop.Domain.Dtoes.Category;
using Shop.Domain.Dtoes.Product;
using Shop.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<CategoryDto> GetCategories()
        {
            var data = _productRepository.GetCategories();
            var result = new List<CategoryDto>();
            foreach(var item in data)
            {
                result.Add(new CategoryDto
                {
                    CategoryId = item.CategoryId,
                    CategoryName = item.CategoryName,
                    Products = GetProductsByCategoryId(item.CategoryId)
                });
            }
            return result;
        }

        public List<ProductDto> GetProductsByCategoryId(int categoryId)
        {
            var data = _productRepository.GetProductsByCategoryId(categoryId);
            return data.Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                IsActive = p.IsActive
            }).ToList();
        }
    }
}
