using lab3.Models;
using lab3.Services;
using Microsoft.AspNetCore.Mvc;

namespace lab3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IProductService productService,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            _logger.LogInformation("Getting all products");

            var products = _productService.GetAll();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _productService.GetById(id);

            if (product == null)
            {
                _logger.LogWarning(
                    "Product with ID {ProductId} was not found", id);

                return NotFound();
            }

            _logger.LogInformation(
                "Product with ID {ProductId} was found", id);

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> Add(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                return BadRequest("Product name is required.");
            }

            if (product.Price < 0)
            {
                return BadRequest("Product price cannot be negative.");
            }

            var createdProduct = _productService.Add(product);

            _logger.LogInformation(
                "Product {ProductName} was created",
                product.Name);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdProduct.Id },
                createdProduct);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _productService.Delete(id);

            if (!deleted)
            {
                _logger.LogWarning(
                    "Product with ID {ProductId} was not found", id);

                return NotFound();
            }

            _logger.LogInformation(
                "Product with ID {ProductId} was deleted", id);

            return NoContent();
        }
    }
}