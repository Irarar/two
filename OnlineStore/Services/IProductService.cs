using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> Get();
        public Task<Product> GetAsync(Guid productId);
        public Task<Guid> UpdateAsync(Product product);
        public Task<Guid> CreateAsync(Product product);
        public Task DeleteAsync(Guid productId);
        public Task<Product> GettingTheModelAsync(Product product);
        public Task<Product> DeleteTheModelAsync(Guid productId);
    }

    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Product>> Get()
        {
            return await _db.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product> GetAsync(Guid productId)
        {
            return await _db.Products.Include(o => 
                o.Category).FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<Guid> UpdateAsync(Product product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return product.Id;
        }

        public async Task<Guid> CreateAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product.Id;
        }

        public async Task DeleteAsync(Guid productId)
        {
            _db.Products.Remove(_db.Products.AsNoTracking().FirstOrDefault(p=>p.Id == productId));
            await _db.SaveChangesAsync();
        }

        public async Task<Product> GettingTheModelAsync(Product product)
        {
          return await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == product.Id);
            
        }
        public async Task<Product> DeleteTheModelAsync(Guid productId)
        {
            return await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId);

        }
    }
}