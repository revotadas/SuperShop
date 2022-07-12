using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data {
    public class SeedDb {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private Random random;

        public SeedDb(DataContext context, IUserHelper userHelper) {
            this.context = context;
            this.userHelper = userHelper;
            random = new Random();
        }

        public async Task SeedAsync() {
            await context.Database.EnsureCreatedAsync();

            var user = await userHelper.GetUserByEmailAsync("rafael.lopes24@gmail.com");
            if(user == null) {
                user = new User {
                    FirstName = "Rafael",
                    LastName = "Lopes",
                    Email = "rafael.lopes24@gmail.com",
                    UserName = "rafael.lopes24@gmail.com",
                    PhoneNumber = "915272773"
                };

                var result = await userHelper.AddUserAsync(user, "qwerty123");
                if(result != IdentityResult.Success) {
                    throw new InvalidOperationException("Could not create user in seeder.");
                }
            }

            if(!context.Products.Any()) {
                AddProduct("iPhone X", user);
                AddProduct("Magic Mouse", user);
                AddProduct("iWatch Serires 4", user);
                AddProduct("iPad Mini", user);
                await context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name, User user) {
            context.Products.Add(new Product {
                Name = name,
                Price = random.Next(1000),
                IsAvailable = true,
                Stock = random.Next(100),
                User = user
            });
        }
    }
}
