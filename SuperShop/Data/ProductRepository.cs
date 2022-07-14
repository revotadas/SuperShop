using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using System.Linq;

namespace SuperShop.Data {
    public class ProductRepository : GenericRepository<Product>, IProductsRepository {
        private readonly DataContext context;

        public ProductRepository(DataContext context) : base(context) {
            this.context = context;
        }

        public IQueryable GetAllWithUsers() {
            return context.Products.Include(p => p.User);
        }
    }
}
