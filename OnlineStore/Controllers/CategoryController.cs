using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IStoreService _storeService;

        public CategoryController(ICategoryService categoryService, IStoreService storeService)
        {
            _categoryService = categoryService;
            _storeService = storeService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.Get());
        }

        public async Task<IActionResult> Details(Guid categoryId)
        {
            return View(await _categoryService.GetAsync(categoryId));
        }

        public async Task<IActionResult> Edit(Guid categoryId)
        {
            ViewBag.categoryId = new SelectList(await _storeService.Get(), "Id", "Name");
            return View(await _categoryService.GetAsync(categoryId));
        }
        public async Task<IActionResult> Create(Guid categoryId)
        {
            ViewBag.categoryId = new SelectList(await _storeService.Get(), "Id", "Name");
            return View(await _categoryService.GetAsync(categoryId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            var categoryId = await _categoryService.CreateAsync(category);
            return RedirectToAction("Index", "Category", new {categoryId});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            await _categoryService.DeleteAsync(categoryId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            var categoryId = await _categoryService.UpdateAsync(category);
            return RedirectToAction("Details", new { categoryId });
        }
    }
}