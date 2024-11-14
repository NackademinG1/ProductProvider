using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductProvider.Business.Interfaces;
using ProductProvider.Business.Models;

namespace ProductProvider.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public ActionResult<Product> CreateProduct(ProductRequest productRequest)
        {
            var response = _productService.CreateProduct(productRequest);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            if (response.ProductResult == null)
            {
                return BadRequest("Product creation failed: no result was returned.");
            }

            return CreatedAtAction(nameof(GetProduct), new { id = response.ProductResult!.Id }, response.ProductResult);
        }


        // GET: api/products/{id}
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(string id)
        {
            var response = _productService.GetProduct(id);
            if (response.ProductResult == null && !response!.Success)
            {
                return NotFound(response.Message); // Returnera 404 om produkten inte hittas
            }
            return Ok(response.ProductResult);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var response = _productService.GetAllProducts();
            if (!response.Success)
            {
                return StatusCode(500, response.Message);
            }
            return Ok(response.Result);
        }
    }
}
