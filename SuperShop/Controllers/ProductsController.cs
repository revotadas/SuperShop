using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Controllers {
    public class ProductsController : Controller {
        private readonly IProductsRepository productsRepository;
        private readonly IUserHelper userHelper;

        public ProductsController(IProductsRepository productsRepository, IUserHelper userHelper) {
            this.productsRepository = productsRepository;
            this.userHelper = userHelper;
        }

        public IActionResult Index() {
            return View(productsRepository.GetAll().OrderBy(p => p.Name));
        }

        public async Task<IActionResult> Details(int? id) {
            if(id == null) {
                return NotFound();
            }

            var product = await productsRepository.GetByIdAsync(id.Value);
            if(product == null) {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product) {
            if(ModelState.IsValid) {
                product.User = await userHelper.GetUserByEmailAsync("rafael.lopes24@gmail.com");
                await productsRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id) {
            if(id == null) {
                return NotFound();
            }

            var product = await productsRepository.GetByIdAsync(id.Value);

            if(product == null) {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product) {
            if(id != product.Id) {
                return NotFound();
            }

            if(ModelState.IsValid) {
                try {
                    await productsRepository.UpdateAsync(product);
                } catch(DbUpdateConcurrencyException) {
                    if(!await productsRepository.ExistAsync(product.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id) {
            if(id == null) {
                return NotFound();
            }

            var product = await productsRepository.GetByIdAsync(id.Value);

            if(product == null) {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var product = await productsRepository.GetByIdAsync(id);
            await productsRepository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
