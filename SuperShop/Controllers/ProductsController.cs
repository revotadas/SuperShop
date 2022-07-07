using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;
using System.Threading.Tasks;

namespace SuperShop.Controllers {
    public class ProductsController : Controller {
        private readonly IRepository repository;

        public ProductsController(IRepository repository) {
            this.repository = repository;
        }

        public IActionResult Index() {
            return View(repository.GetProducts());
        }

        public IActionResult Details(int? id) {
            if(id == null) {
                return NotFound();
            }

            var product = repository.GetProduct(id.Value);
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
                repository.AddProduct(product);
                await repository.SaveAllAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Edit(int? id) {
            if(id == null) {
                return NotFound();
            }

            var product = repository.GetProduct(id.Value);
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
                    repository.UpdateProduct(product);
                    await repository.SaveAllAsync();
                } catch(DbUpdateConcurrencyException) {
                    if(!repository.ProductExists(product.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Delete(int? id) {
            if(id == null) {
                return NotFound();
            }

            var product = repository.GetProduct(id.Value);
            if(product == null) {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var product = repository.GetProduct(id);
            repository.RemoveProduct(product);
            await repository.SaveAllAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
