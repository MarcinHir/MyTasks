using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTasks.Core.Models;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Services;
using MyTasks.Core.ViewModels;
using MyTasks.Persistance.Extensions;
using MyTasks.Persistance.Services;
using System.Threading.Tasks;

namespace MyTasks.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService;
        private ITaskService _taskService;

        public CategoryController(ICategoryService categoryService, ITaskService taskService)
        {
                _categoryService = categoryService;
                _taskService = taskService;
        }
        public IActionResult Category(int id = 0)
        {
            var userId = User.GetUserId();

            var category = id == 0 ? new Category { Id = 0, UserId = userId } : _categoryService.GetCategory(id, userId); 
            
            var vm = new CategoryViewModel
            {
                Category = category,
                Heading = id == 0 ?
                "Dodawanie nowej Kategorii" :
                "Edycja Kategorii",
                
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Category(Category category)
        {
            var userId = User.GetUserId();
            category.UserId = userId;
            if (!ModelState.IsValid)
            {
                var vm = new CategoryViewModel
                {
                    Category = category,
                    Heading = category.Id == 0 ?
                "Dodawanie nowej kategorii" :
                "Edycja kategorii"                 
                };
                return View("Category", vm);
            }

            if (category.Id == 0)
                _categoryService.Add(category);
            else
                _categoryService.Update(category);

            return RedirectToAction("Categories");
        }

        public IActionResult Categories()
        {
            var userId = User.GetUserId();

            var vm = new CategoriesViewModel
            {
                Categories = _categoryService.GetCategories(userId)
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var userId = User.GetUserId();
            bool isUsed = _categoryService.IsUsed(id);
            if (!isUsed)
            {
                try
                {
                    _categoryService.Delete(id, userId);
                }

                catch (Exception ex)
                {
                    //logowanie do pliku

                    return Json(new { success = false, message = ex.Message });
                }
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Wybrana kategoria jest używana, najpierw usuń zadania do niej przypisane!" });
            }
        }

    }
}
