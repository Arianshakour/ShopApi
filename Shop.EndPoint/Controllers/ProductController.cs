using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services.Interfaces;
using Shop.Domain.Dtoes.Product;
using Shop.Domain.Dtoes;

namespace Shop.EndPoint.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public IActionResult GetProducts([FromQuery] FilteringDto filter)
        {
            var model = _productService.GetAllProducts(filter);
            return Ok(model);
        }

        [HttpGet("ByCategory/{categoryId}")]
        public IActionResult GetProductsByCategoryId(int categoryId)
        {
            if (!_productService.IsExistCategory(categoryId))
            {
                return NotFound();
            }
            var model = _productService.GetProductsByCategoryId(categoryId);
            return Ok(model);
        }
        //ba ravesh paein ham mishe faqat nabayad 2 ta get sade bashe chon hang mikone 
        //route ra bayad avaz koni ba query ham nemishe injoori [HttpGet("ByCategory/{categoryId}")]
        //[HttpGet]
        //public IActionResult GetProductsByCategoryId([FromQuery] int categoryId)
        //{
        //    if (!_productService.IsExistCategory(categoryId))
        //    {
        //        return NotFound();
        //    }
        //    var model = _productService.GetProductsByCategoryId(categoryId);
        //    return Ok(model);
        //}
        [HttpGet("{id}", Name = "GetProduct")]
        public IActionResult GetProduct(int id)
        {
            if (!_productService.IsExistProduct(id))
            {
                return NotFound();
            }
            var model = _productService.GetProduct(id);
            return Ok(model);
        }
        //[HttpGet]
        //public IActionResult GetProductByCategoryId([FromQuery] int categoryId,int id)
        //{
        //    if (!_productService.IsExistCategory(categoryId))
        //    {
        //        return NotFound();
        //    }
        //    if (!_productService.IsExistProduct(id))
        //    {
        //        return NotFound();
        //    }
        //    var model = _productService.GetProductByCategoryId(categoryId,id);
        //    if (model == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(model);
        //}
        [HttpPost]
        public IActionResult Create(int categoryId, ProductForCreateDto create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!_productService.IsExistCategory(categoryId))
            {
                return NotFound();
            }
            var model = _productService.AddProduct(categoryId, create);
            return Ok(model);
        }
        [HttpPut("{productId}")]
        public IActionResult Edit(int categoryId, int productId, ProductForEditDto edit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!_productService.IsExistCategory(categoryId))
            {
                return NotFound();
            }
            var model = _productService.UpdateProduct(categoryId, productId, edit);
            if (model == false)
            {
                return NotFound();
            }
            return Ok();
        }
        [HttpDelete("{productId}")]
        public IActionResult Delete(int productId)
        {
            if (!_productService.IsExistProduct(productId))
            {
                return NotFound();
            }
            var model = _productService.DeleteProduct(productId);
            if (model == false)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
