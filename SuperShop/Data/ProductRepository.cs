using SuperShop.Data.Entities;

namespace SuperShop.Data {
    public class ProductRepository : GenericRepository<Product>, IProductsRepository {
        public ProductRepository(DataContext context) : base(context) {

        }
    }
}
