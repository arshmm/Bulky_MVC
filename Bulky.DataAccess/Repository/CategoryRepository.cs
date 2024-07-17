﻿using BulkyBookBook.DataAccess.Data;
using BulkyBookBook.DataAccess.Repository.IRepository;
using BulkyBookBook.Models;

namespace BulkyBookBook.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
