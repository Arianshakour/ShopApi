using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shop.Domain.Common;
using Shop.Domain.Dtoes;
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

        public Category? GetCategory(int id)
        {
            var c = _context.Categorys.FirstOrDefault(x => x.CategoryId == id);
            //if (c == null)
            //{
            //    throw new NullReferenceException();
            //}
            return c;
        }

        public Category GetCategory2(int id, bool includeProducts)
        {
            if (includeProducts)
            {
                var c2 = _context.Categorys.Include(x => x.products).FirstOrDefault(x => x.CategoryId == id);
                if (c2 == null)
                {
                    throw new NullReferenceException();
                }
                return c2;
            }
            var c = _context.Categorys.FirstOrDefault(x => x.CategoryId == id);
            if (c == null)
            {
                throw new NullReferenceException();
            }
            return c;

        }

        public Product? GetProduct(int id)
        {
            var p = _context.Products.FirstOrDefault(x => x.Id == id);
            //if (p == null)
            //{
            //    throw new NullReferenceException();
            //}
            return p;
        }

        public Product GetProductByCategoryId(int categoryId, int productId)
        {
            var p = _context.Products.FirstOrDefault(x => x.Id == productId && x.CategoryId == categoryId);
            //if (p == null)
            //{
            //    throw new NullReferenceException();
            //}
            return p;
        }

        public bool IsExistProduct(int id)
        {
            return _context.Products.Any(x => x.Id == id);
        }
        public List<Product> GetProducts(FilteringDto filter)
        {
            var products = _context.Products.OrderBy(p => p.Id).AsQueryable();
            if (!string.IsNullOrEmpty(filter.Search))
            {
                products = products.Where(p => p.ProductName.Contains(filter.Search)
                || p.Description.Contains(filter.Search));
            }
            if (filter.MinPrice.HasValue)
            {
                products = products.Where(x => x.Price >= filter.MinPrice.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                products = products.Where(x => x.Price <= filter.MaxPrice.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                products = products.Where(x => x.Price <= filter.MaxPrice.Value);
            }
            if(filter.CategoryId != null)
            {
                products = products.Where(x => x.CategoryId == filter.CategoryId);
            }

            // impelement dynamic sort
            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                //in if paein check mikone ke property ke dare bahash sort mikone baraye Product hast ya na
                //masalan Price baraye Product hast
                if(typeof(Product).GetProperty(filter.SortBy) != null)
                {
                    products = products.OrderByCustom(filter.SortBy, filter.SortOrder);
                }
            }

            var skip = (filter.PageId - 1) * filter.PageSize;
            products = products.Skip(skip).Take(filter.PageSize);
            return products.ToList();
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
