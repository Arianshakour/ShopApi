using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services.Interfaces;

namespace Shop.EndPoint.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly IProductService _productService;
        public CategoryController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public IActionResult GetCategories()
        {
            var model = _productService.GetCategories();
            return Ok(model);
        }
    }
}
