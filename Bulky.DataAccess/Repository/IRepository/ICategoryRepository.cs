using BulkyBookBook.Models;

namespace BulkyBookBook.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void update(Category obj);


    }
}
