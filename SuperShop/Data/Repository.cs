using SuperShop.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data {
    public class Repository : IRepository {
        private readonly DataContext context;

        public Repository(DataContext context) {
            this.context = context;
        }

        public IEnumerable<Product> GetProducts() {
            return context.Products.OrderBy(p => p.Name);
        }

        public Product GetProduct(int id) {
            return context.Products.Find(id);
        }

        public void AddProduct(Product product) {
            context.Products.Add(product);
        }

        public void UpdateProduct(Product product) {
            context.Products.Update(product);
        }

        public void RemoveProduct(Product product) {
            context.Products.Remove(product);
        }

        public async Task<bool> SaveAllAsync() {
            return await context.SaveChangesAsync() > 0;
        }

        public bool ProductExists(int id) {
            return context.Products.Any(p => p.Id == id);
        }
    }
}
