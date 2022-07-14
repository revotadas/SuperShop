using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Data;

namespace SuperShop.Controllers.API {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller {
        private readonly IProductsRepository productsRepository;

        public ProductsController(IProductsRepository productsRepository) {
            this.productsRepository = productsRepository;
        }

        [HttpGet]
        public IActionResult GetProducts() {
            return Ok(productsRepository.GetAllWithUsers());
        }
    }
}
