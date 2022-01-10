using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IStoreService
    {
        public Task<IEnumerable<Store>> Get();
        public Task<Store> GetAsync(Guid storeId);
    }

    class StoreService : IStoreService
    {
        private readonly AppDbContext _db;

        public StoreService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Store>> Get()
        {
            return await _db.Store.ToListAsync();
        }

        public async Task<Store> GetAsync(Guid storeId)
        {
            return await _db.Store.FirstOrDefaultAsync(o => o.Id == storeId);
        }
    }
}
