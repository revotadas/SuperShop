using SuperShop.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data {
    public class SeedDb {
        private readonly DataContext context;
        private Random random;

        public SeedDb(DataContext context) {
            this.context = context;
            random = new Random();
        }

        public async Task SeedAsync() {
            await context.Database.EnsureCreatedAsync();

            if(!context.Products.Any()) {
                AddProduct("iPhone X");
                AddProduct("Magic Mouse");
                AddProduct("iWatch Serires 4");
                AddProduct("iPad Mini");
                await context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name) {
            context.Products.Add(new Product {
                Name = name,
                Price = random.Next(1000),
                IsAvailable = true,
                Stock = random.Next(100)
            });
        }
    }
}
