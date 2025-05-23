using Shop.Application.Services.Interfaces;
using Shop.Domain.Dtoes;
using Shop.Domain.Dtoes.Category;
using Shop.Domain.Dtoes.Product;
using Shop.Domain.Entities;
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
            foreach (var item in data)
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

        public CategoryDto GetCategory(int categoryId)
        {
            var data = _productRepository.GetCategory(categoryId);
            if (data == null)
            {
                return null;
            }
            var result = new CategoryDto()
            {
                CategoryId = data.CategoryId,
                CategoryName = data.CategoryName,
                Products = GetProductsByCategoryId(data.CategoryId)
            };
            return result;
        }

        public CategoryDto GetCategory2(int categoryId, bool includeProducts)
        {
            if (includeProducts)
            {
                var data2 = _productRepository.GetCategory2(categoryId, includeProducts);
                var result2 = new CategoryDto()
                {
                    CategoryId = data2.CategoryId,
                    CategoryName = data2.CategoryName,
                    Products = data2.products.Select(x => new ProductDto
                    {
                        Id = x.Id,
                        ProductName = x.ProductName,
                        Price = x.Price,
                        Description = x.Description,
                        IsActive = x.IsActive,
                        CategoryId = data2.CategoryId
                    }).ToList()
                };
                return result2;
            }
            var data = _productRepository.GetCategory2(categoryId, includeProducts);
            var result = new CategoryDto()
            {
                CategoryId = data.CategoryId,
                CategoryName = data.CategoryName
            };
            return result;
        }

        public CategoryDto AddCategory(CategoryForCreateDto category)
        {
            var c = new Category()
            {
                CategoryName = category.CategoryName
            };
            _productRepository.AddCategory(c);
            _productRepository.Save();
            var created = new CategoryDto()
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            };
            return created;
        }

        public bool UpdateCategory(int id, CategoryForEditDto category)
        {
            var c = _productRepository.GetCategory(id);
            if (c == null)
            {
                return false;
            }
            else
            {
                c.CategoryName = category.CategoryName;
                _productRepository.UpdateCategory(c);
                _productRepository.Save();
                return true;
            }

        }

        public bool DeleteCategory(int id)
        {
            var c = _productRepository.GetCategory(id);
            if (c == null)
            {
                return false;
            }
            else
            {
                _productRepository.DeleteCategory(c);
                _productRepository.Save();
                return true;
            }
        }

        public bool IsExistCategory(int id)
        {
            return _productRepository.IsExistCategory(id);
        }


        //Product

        public List<ProductDto> GetProductsByCategoryId(int categoryId)
        {
            var data = _productRepository.GetProductsByCategoryId(categoryId);
            return data.Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId
            }).ToList();
        }


        public List<ProductDto> GetAllProducts(FilteringDto filter)
        {
            var data = _productRepository.GetProducts(filter);
            return data.Select(x => new ProductDto
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                Price = x.Price,
                ProductName = x.ProductName,
                Description = x.Description,
                IsActive = x.IsActive
            }).ToList();
        }
        public ProductDto GetProductByCategoryId(int categoryId, int id)
        {
            var data = _productRepository.GetProductByCategoryId(categoryId, id);
            if (data == null)
            {
                return null;
            }
            var result = new ProductDto()
            {
                Id = data.Id,
                ProductName = data.ProductName,
                Description = data.Description,
                IsActive = data.IsActive,
                Price = data.Price,
                CategoryId = data.CategoryId
            };
            return result;
        }
        public ProductDto GetProduct(int id)
        {
            var data = _productRepository.GetProduct(id);
            var result = new ProductDto()
            {
                Id = data.Id,
                ProductName = data.ProductName,
                Description = data.Description,
                IsActive = data.IsActive,
                Price = data.Price,
                CategoryId = data.CategoryId
            };
            return result;
        }

        public ProductDto AddProduct(int categoryId, ProductForCreateDto create)
        {
            var p = new Product()
            {
                ProductName = create.ProductName,
                Price = create.Price,
                IsActive = create.IsActive,
                Description = create.Description,
                CategoryId = categoryId
            };
            _productRepository.AddProduct(p);
            _productRepository.Save();
            var created = new ProductDto()
            {
                CategoryId = p.CategoryId,
                ProductName = p.ProductName,
                IsActive = p.IsActive,
                Description = p.Description,
                Price = p.Price
            };
            return created;
        }

        public bool UpdateProduct(int categoryId, int productId, ProductForEditDto edit)
        {
            var p = _productRepository.GetProductByCategoryId(categoryId, productId);
            if (p == null)
            {
                return false;
            }
            else
            {
                if (p.CategoryId != edit.CategoryId)
                {
                    if (!_productRepository.IsExistCategory(edit.CategoryId))
                    {
                        return false;
                    }
                }
                p.ProductName = edit.ProductName;
                p.Price = edit.Price;
                p.IsActive = edit.IsActive;
                p.Description = edit.Description;
                p.CategoryId = edit.CategoryId;
                _productRepository.UpdateProduct(p);
                _productRepository.Save();
                return true;
            }
        }

        public bool DeleteProduct(int productId)
        {
            var p = _productRepository.GetProduct(productId);
            if (p == null)
            {
                return false;
            }
            else
            {
                _productRepository.DeleteProduct(p);
                _productRepository.Save();
                return true;
            }
        }
        public bool IsExistProduct(int id)
        {
            return _productRepository.IsExistProduct(id);
        }


    }
}
