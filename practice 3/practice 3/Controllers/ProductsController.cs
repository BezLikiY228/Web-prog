using Microsoft.AspNetCore.Mvc;
using practice_3.Services;

namespace practice_3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductService service,
        ILogger<ProductsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        _logger.LogInformation("Getting all products");

        return Ok(_service.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = _service.GetById(id);

        if (product == null)
        {
            _logger.LogWarning(
                "Product with ID {ProductId} was not found",
                id);

            return NotFound();
        }

        _logger.LogInformation(
            "Product with ID {ProductId} was found",
            id);

        return Ok(product);
    }
}