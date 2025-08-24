using Microsoft.AspNetCore.Mvc;
using closymyx.DTOs;
using closymyx.Services;

namespace closymyx.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            try
            {
                var product = await _service.CreateProductAsync(dto);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{productId}/similar")]
        public async Task<IActionResult> GetSimilar(Guid productId)
        {
            try
            {
                var products = await _service.GetSimilarProductsAsync(productId);
                if (products.Count == 0)
                    return NotFound(new { message = "Похожие товары не найдены." });

                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}