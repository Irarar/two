using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;
using OnlineStore.Models.ViewModel;

namespace OnlineStore.Services
{
    public interface IHomeService
    {
        public Task<HomeVM> GetAsync();
        public Task<DetailsVM> GetDetailsAsync(Guid productId);

    }

    public class HomeService : IHomeService
    {
        private readonly AppDbContext _db;

        public HomeService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<HomeVM> GetAsync()
        {
            return new HomeVM
            {
                Product = await (_db.Products.Include(p => p.Category).ThenInclude(p => p.Store)).ToListAsync(),
                Category = await _db.Category.ToListAsync()

            };
        }

        public async Task<DetailsVM> GetDetailsAsync(Guid productId)
        {
            return new DetailsVM()
            {
                Product = await (_db.Products.Include(p =>
                    p.Category).ThenInclude(p => p.Store).Where(p =>
                    p.Id == productId).FirstOrDefaultAsync()),
                Basket = false
            };
        }

    }
}
