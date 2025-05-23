using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services.Interfaces;
using Shop.Domain.Dtoes.Category;

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

        [HttpGet("{id}", Name = "GetCategory")]
        public IActionResult GetCategory(int id)
        {
            var model = _productService.GetCategory(id);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }
        //ravesh zir ham mishe taein koni ke product ra mikhad ya na
        //[HttpGet("{id}", Name = "GetCategory2")]
        //public IActionResult GetCategory2(int id, bool includeProducts = false)
        //{
        //    var model = _productService.GetCategory2(id, includeProducts);
        //    if (model == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(model);
        //}
        [HttpPost]
        public IActionResult Create([FromBody] CategoryForCreateDto create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var model = _productService.AddCategory(create);
            return Ok(model);
        }
        [HttpPut("{id}")]
        public IActionResult Edit(int id, CategoryForEditDto edit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var model = _productService.UpdateCategory(id, edit);
            if (model == false)
            {
                return NotFound();
            }
            return Ok();
            //return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var model = _productService.DeleteCategory(id);
            if (model == false)
            {
                return NotFound();
            }
            return Ok();
            //return NoContent();
        }
    }
}
