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
        private readonly IImageHelper imageHelper;
        private readonly IConverterHelper converterHelper;

        public ProductsController(IProductsRepository productsRepository, IUserHelper userHelper, IImageHelper imageHelper, IConverterHelper converterHelper) {
            this.productsRepository = productsRepository;
            this.userHelper = userHelper;
            this.imageHelper = imageHelper;
            this.converterHelper = converterHelper;
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
                    path = await imageHelper.UploadImageAsync(model.ImageFile, "products");
                }

                var product = converterHelper.ToProduct(model, path, true);

                product.User = await userHelper.GetUserByEmailAsync("rafael.lopes24@gmail.com");

                await productsRepository.CreateAsync(product);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id) {
            if(id == null) {
                return NotFound();
            }

            var product = await productsRepository.GetByIdAsync(id.Value);

            if(product == null) {
                return NotFound();
            }

            var model = converterHelper.ToProductViewModel(product);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model) {
            if(ModelState.IsValid) {
                try {
                    var path = model.ImageUrl;

                    if(model.ImageFile != null && model.ImageFile.Length > 0) {
                        path = await imageHelper.UploadImageAsync(model.ImageFile, "products");
                    }

                    var product = converterHelper.ToProduct(model, path, false);

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
