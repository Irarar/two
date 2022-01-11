using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data;
using OnlineStore.Models;
using OnlineStore.Models.ViewModel;
using OnlineStore.Utility;
using System.Text;
using System.Threading.Tasks;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _web;
        private readonly IEmailService _email;

        [BindProperty]
        public ProductModelVM ProductModel { get; set; }
        public BasketController(AppDbContext db, IWebHostEnvironment web, IEmailService email)
        {
            _db = db;
            _web = web;
            _email = email;
        }

        public IActionResult Index()
        {
            List<ShoppingBasket> shoppingBaskets = new List<ShoppingBasket>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingBasket>>(WC.SessionBasket) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingBasket>>(WC.SessionBasket).Count() > 0)
            {
                shoppingBaskets = HttpContext.Session.Get<List<ShoppingBasket>>(WC.SessionBasket);
            }

            List<Guid> shoppingIds = shoppingBaskets.Select(p => p.ProductId).ToList();
            IEnumerable<Product> productList = _db.Products.Where(p => shoppingIds.Contains(p.Id));

            return View(productList);
        }

        public IActionResult Delete(Guid productId)
        {
            List<ShoppingBasket> shoppingBaskets = new List<ShoppingBasket>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingBasket>>(WC.SessionBasket) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingBasket>>(WC.SessionBasket).Count() > 0)
            {
                shoppingBaskets = HttpContext.Session.Get<List<ShoppingBasket>>(WC.SessionBasket);
            }

            shoppingBaskets.Remove(shoppingBaskets.FirstOrDefault(p => p.ProductId == productId));
            HttpContext.Session.Set(WC.SessionBasket, shoppingBaskets);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction("Summary");
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            List<ShoppingBasket> shoppingBaskets = new List<ShoppingBasket>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingBasket>>(WC.SessionBasket) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingBasket>>(WC.SessionBasket).Count() > 0)
            {
                shoppingBaskets = HttpContext.Session.Get<List<ShoppingBasket>>(WC.SessionBasket);
            }
            List<Guid> shoppingIds = shoppingBaskets.Select(p => p.ProductId).ToList();
            IEnumerable<Product> productList = _db.Products.Where(p => shoppingIds.Contains(p.Id));
            ProductModel = new ProductModelVM()
            {
                User = _db.Users.FirstOrDefault(p => p.Id == claim.Value),
                ProductList = productList.ToList(),
            };

            return View(ProductModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductModelVM productModelVm)
        {
            var PathToTemplate = _web.WebRootPath + Path.DirectorySeparatorChar +
                                 "templates" + Path.DirectorySeparatorChar + "Inquiry.html";
            var subject = "New Inquiry";
            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }

            StringBuilder productListSB = new StringBuilder();
            foreach (var pro in productModelVm.ProductList)
            {
                productListSB.Append(
                    $" - Name: {pro.Name} <span style='font-size:14px;'> (ID: {pro.Id}) </span> <br/> ");
            }

            string messageBody = string.Format(HtmlBody,
                productModelVm.User.Login,
                productModelVm.User.Email,
                productListSB);

            await _email.SendEmailAsync(Role.AdminEmail, subject, messageBody);
            return RedirectToAction("InquiryConfirmation");
        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();
            return View();
        }
    }
}
