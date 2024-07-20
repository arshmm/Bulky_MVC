using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBookBook.DataAccess.Data;

namespace BulkyBookBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void update(Product obj)
        {
            _db.Products.Update(obj);
        }
    }
}
