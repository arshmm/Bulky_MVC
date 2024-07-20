using BulkyBook.Models;
using BulkyBookBook.DataAccess.Repository.IRepository;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void update(Product obj);
    }
}


