using DominandoEFCore17.Data;
using DominandoEFCore17.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DominandoEFCore17.Controllers
{
    [ApiController]
    [Route("{tenant}/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Product> Get([FromServices]ApplicationDbContext db)
        {
            var products = db.Products.ToArray();

            return products;
        }
    }
}
