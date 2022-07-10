using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;
using System.Threading.Tasks;

namespace SuperShop.Controllers {
    public class ProductsController : Controller {
        private readonly IProductsRepository productsRepository;

        public ProductsController(IProductsRepository productsRepository) {
            this.productsRepository = productsRepository;
        }

        public IActionResult Index() {
            return View(productsRepository.GetAll());
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
