using SuperShop.Data.Entities;
using System.Linq;

namespace SuperShop.Data {
    public interface IProductsRepository : IGenericRepository<Product> {
        public IQueryable GetAllWithUsers();
    }
}
