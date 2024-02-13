using Microsoft.AspNetCore.Mvc;
using WepPha2.Interfaces;
using WepPha2.Models;
using WepPha2.Repository;
using WepPha2.ViewModels;

namespace WepPha2.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAll();
            return View(categories);
        }
        public  async Task<IActionResult> Create()
        {
            return  View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            _categoryRepository.Add(category);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var categoryDetails = await _categoryRepository.GetCategoryById(id);
            if (categoryDetails == null) return View("Error");
            return View(categoryDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var categoryDetails = await _categoryRepository.GetCategoryById(id);

            if (categoryDetails == null)
            {
                return View("Error");
            }
            _categoryRepository.Delete(categoryDetails);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null) return View("Error");
            var categoryVM = new EditCategoryViewModel
            {
                CategoryId= category.CategoryId,
                CategoryName = category.CategoryName
            };
            return View(categoryVM);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Edit(int id, EditCategoryViewModel categoryVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit category");
                return View(categoryVM);
            }

            var userCategory = await _categoryRepository.GetCategoryByIdAsNoTracking(id);

            if (userCategory == null)
            {
                return View("Error");
            }

            var category = new Category
            {
                CategoryId = id,
                CategoryName = categoryVM.CategoryName,
                
            };

            _categoryRepository.Update(category);

            return RedirectToAction("Index");
        }
    }
}
