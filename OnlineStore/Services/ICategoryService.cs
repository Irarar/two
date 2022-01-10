using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface ICategoryService
    {
        public Task<IEnumerable<Category>> Get();
        public Task<Category> GetAsync(Guid categoryId);
        public Task<Guid> UpdateAsync(Category category);
        public Task<Guid> CreateAsync(Category category);
        public Task DeleteAsync(Guid categoryId);

    }

    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _db;

        public CategoryService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> Get()
        {
            return await _db.Category.Include(p => 
                p.Store).ToListAsync();
        }

        public async Task<Category> GetAsync(Guid categoryId)
        {
            return await _db.Category.AsNoTracking().Include(p => 
                p.Store).FirstOrDefaultAsync(o => o.Id == categoryId);
        }

        public async Task<Guid> UpdateAsync(Category category)
        {
            _db.Update(category);
            await _db.SaveChangesAsync();
            return category.Id;
        }

        public async Task<Guid> CreateAsync(Category category)
        {
            _db.Add(category);
            await _db.SaveChangesAsync();
            return category.Id;
        }

        public async Task DeleteAsync(Guid categoryId)
        {
            _db.Category.Remove( _db.Category.AsNoTracking().
                FirstOrDefault(p => p.Id == categoryId));
            await _db.SaveChangesAsync();
        }
    }
}