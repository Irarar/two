using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Models.ViewModel;
using OnlineStore.Services;
using OnlineStore.Utility;

namespace OnlineStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _home;

        public HomeController(IHomeService home)
        {
            _home = home;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _home.GetAsync());
        }

        public async Task<IActionResult> Details(Guid productId)
        {
            List<ShoppingBasket> shoppingBaskets = new List<ShoppingBasket>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingBasket>>(WC.SessionBasket) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingBasket>>(WC.SessionBasket).Count() > 0)
            {
                shoppingBaskets = HttpContext.Session.Get<List<ShoppingBasket>>(WC.SessionBasket);
            }

            var wm = await _home.GetDetailsAsync(productId);
            foreach (var VARIABLE in shoppingBaskets)
            {
                if (VARIABLE.ProductId == productId)
                {
                    wm.Basket = true;
                }
            }
            return View(wm);

        }
        
        [HttpPost(),ActionName("Details")]
        public async Task<IActionResult> DetailsPost(Guid productId)
        {
            List<ShoppingBasket> shoppingBaskets = new List<ShoppingBasket>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingBasket>>(WC.SessionBasket) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingBasket>>(WC.SessionBasket).Count() > 0)
            {
               shoppingBaskets = HttpContext.Session.Get<List<ShoppingBasket>>(WC.SessionBasket);
            }
            shoppingBaskets.Add(new ShoppingBasket { ProductId = productId} );
            HttpContext.Session.Set(WC.SessionBasket,shoppingBaskets);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> RemoveFromBasker(Guid productId)
        {
            List<ShoppingBasket> shoppingBaskets = new List<ShoppingBasket>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingBasket>>(WC.SessionBasket) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingBasket>>(WC.SessionBasket).Count() > 0)
            {
                shoppingBaskets = HttpContext.Session.Get<List<ShoppingBasket>>(WC.SessionBasket);
            }

            var remove = shoppingBaskets.SingleOrDefault(p => p.ProductId == productId);
            if (remove != null)
            {
                shoppingBaskets.Remove(remove);
            }
            HttpContext.Session.Set(WC.SessionBasket, shoppingBaskets);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
