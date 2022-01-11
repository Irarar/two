using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    [Authorize(Roles = Role.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _web;

        public ProductController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment web)
        {
            _productService = productService;
            _categoryService = categoryService;
            _web = web;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productService.Get());
        }

        public async Task<IActionResult> Details(Guid productId)
        {
            return View(await _productService.GetAsync(productId));
        }

        public async Task<IActionResult> Create(Guid productId)
        {
            ViewBag.productId = new SelectList(await _categoryService.Get(), "Id", "Name");
            return View(await _productService.GetAsync(productId));
        }

        public async Task<IActionResult> Edit(Guid productId)
        {
            ViewBag.productId = new SelectList(await _categoryService.Get(), "Id", "Name");
            return View(await _productService.GetAsync(productId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _web.WebRootPath;
            string ImagePath = @"\images\products\";
            var objFromDb = _productService.GettingTheModelAsync(product);

            if (files.Count > 0)
            {
                string upload = webRootPath + ImagePath;
                string filename = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);
                var oldFile = Path.Combine(upload,objFromDb.Result.Image);

                if (System.IO.File.Exists(webRootPath+objFromDb.Result.Image))
                {
                    System.IO.File.Delete(webRootPath + objFromDb.Result.Image);
                }

                using (var fileStream = new FileStream(Path.Combine(upload, filename + extension),
                           FileMode.Create))
                {
                   await files[0].CopyToAsync(fileStream);
                }

                product.Image = ImagePath + filename + extension;
            }
            else
            {
                product.Image = objFromDb.Result.Image;
            }

            var productId = await _productService.UpdateAsync(product);
            return RedirectToAction("Details", new {productId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            string ImagePath = @"\images\products\";
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _web.WebRootPath;
            string upload = webRootPath + ImagePath;
            string filename = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);

            using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }

            product.Image =ImagePath + filename + extension;

            var productId = await _productService.CreateAsync(product);
            return RedirectToAction("Index" ,"Product", new {productId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid productId)
        {
            var objFromDb = _productService.DeleteTheModelAsync(productId);
            string ImagePath = @"\images\products\";
            string upload = _web.WebRootPath + ImagePath;
            string filename = Guid.NewGuid().ToString();
            var oldFile = Path.Combine(upload, objFromDb.Result.Image );

            if (System.IO.File.Exists(_web.WebRootPath + objFromDb.Result.Image))
            {
                System.IO.File.Delete(_web.WebRootPath + objFromDb.Result.Image);
            }
            await _productService.DeleteAsync(productId);
            return RedirectToAction("Index");
        }
    }
}
