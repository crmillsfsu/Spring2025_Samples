using Library.eCommerce.DTO;
using Library.eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Spring2025_Samples.Models;

namespace Api.eCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {

        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Item?> Get()
        {
            return new List<Item?>
            {
                new Item{ Product = new ProductDTO{Id = 1, Name ="Product 1"}, Id = 1, Quantity = 1 },
                new Item{ Product = new ProductDTO{Id = 2, Name ="Product 2"}, Id = 2 , Quantity = 2 },
                new Item{ Product = new ProductDTO{Id = 3, Name ="Product 3"}, Id=3 , Quantity = 3 }
            };
        }
    }
}
