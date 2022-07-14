using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;
using System;
using System.IO;
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
        public async Task<IActionResult> Create(ProductViewModel model) {
            if(ModelState.IsValid) {
                var path = string.Empty;

                if(model.ImageFile != null && model.ImageFile.Length > 0) {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products", file);

                    using(var stream = new FileStream(path, FileMode.Create)) {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/products/{file}";
                }

                var product = ToProduct(model, path);

                product.User = await userHelper.GetUserByEmailAsync("rafael.lopes24@gmail.com");

                await productsRepository.CreateAsync(product);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        private Product ToProduct(ProductViewModel model, string path) {
            return new Product {
                Id = model.Id,
                ImageUrl = path,
                IsAvailable = model.IsAvailable,
                LastPurchase = model.LastPurchase,
                LastSale = model.LastPurchase,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                User = model.User
            };
        }

        public async Task<IActionResult> Edit(int? id) {
            if(id == null) {
                return NotFound();
            }

            var product = await productsRepository.GetByIdAsync(id.Value);

            if(product == null) {
                return NotFound();
            }

            var model = ToProductViewModel(product);

            return View(model);
        }

        private ProductViewModel ToProductViewModel(Product product) {
            return new ProductViewModel {
                Id = product.Id,
                IsAvailable = product.IsAvailable,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model) {
            if(ModelState.IsValid) {
                try {
                    var path = model.ImageUrl;

                    if(model.ImageFile != null && model.ImageFile.Length > 0) {
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products", file);

                        using(var stream=new FileStream(path, FileMode.Create)) {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/products/{file}";
                    }

                    var product = ToProduct(model, path);

                    await productsRepository.UpdateAsync(product);
                } catch(DbUpdateConcurrencyException) {
                    if(!await productsRepository.ExistAsync(model.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
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
