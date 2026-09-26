using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(
            IProductService productService,
            ILogger<ProductsController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            try
            {
                logger.LogInformation("Getting all products");

                return Ok(productService.GetAll());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting all products");
                return StatusCode(500, "Произошла ошибка при получении товаров");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            try
            {
                var product = productService.GetById(id);

                if (product == null)
                {
                    logger.LogWarning(
                        "Product with ID {ProductId} was not found",
                        id);

                    return NotFound();
                }

                logger.LogInformation(
                    "Product with ID {ProductId} was found",
                    id);

                return Ok(product);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error while processing product with ID {ProductId}",
                    id);

                return StatusCode(500, "Произошла ошибка при обработке товара");
            }
        }

        [HttpPost]
        public ActionResult<Product> Add(Product product)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(product.Name))
                {
                    return BadRequest("Название товара не может быть пустым");
                }

                if (product.Price < 0)
                {
                    return BadRequest("Цена не может быть отрицательной");
                }

                var newProduct = productService.Add(product);

                logger.LogInformation(
                    "Product {ProductName} was created",
                    product.Name);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = newProduct.Id },
                    newProduct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while creating product");
                return StatusCode(500, "Произошла ошибка при создании товара");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var deleted = productService.Delete(id);

                if (!deleted)
                {
                    logger.LogWarning(
                        "Product with ID {ProductId} was not found",
                        id);

                    return NotFound();
                }

                logger.LogInformation(
                    "Product with ID {ProductId} was deleted",
                    id);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error while processing product with ID {ProductId}",
                    id);

                return StatusCode(500, "Произошла ошибка при удалении товара");
            }
        }
    }
}