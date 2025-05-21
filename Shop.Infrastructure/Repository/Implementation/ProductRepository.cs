using Shop.Domain.Entities;
using Shop.Infrastructure.Context;
using Shop.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.Repository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopContext _context;
        public ProductRepository(ShopContext context)
        {
            _context = context;
        }
        public void AddCategory(Category category)
        {
            _context.Categorys.Add(category);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public void DeleteCategory(Category category)
        {
            _context.Categorys.Remove(category);
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        public List<Category> GetCategories()
        {
            return _context.Categorys.ToList();
        }

        public Category GetCategory(int id)
        {
            var c = _context.Categorys.FirstOrDefault(x => x.CategoryId == id);
            if (c == null)
            {
                throw new NullReferenceException();
            }
            return c;
        }

        public Product GetProduct(int id)
        {
            var p = _context.Products.FirstOrDefault(x => x.Id == id);
            if (p == null)
            {
                throw new NullReferenceException();
            }
            return p;
        }

        public Product GetProductByCategoryId(int categoryId, int productId)
        {
            var p = _context.Products.FirstOrDefault(x => x.Id == productId && x.CategoryId == categoryId);
            if (p == null)
            {
                throw new NullReferenceException();
            }
            return p;
        }

        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            return _context.Products.Where(x => x.CategoryId == categoryId).ToList();
        }

        public bool IsExistCategory(int id)
        {
            return _context.Categorys.Any(x => x.CategoryId == id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _context.Categorys.Update(category);
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }
    }
}
