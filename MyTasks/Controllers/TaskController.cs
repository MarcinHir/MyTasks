using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTasks.Core.Models;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Services;
using MyTasks.Core.ViewModels;
using MyTasks.Persistance;
using MyTasks.Persistance.Extensions;
using MyTasks.Persistance.Repositories;
using MyTasks.Persistance.Services;
using System.Security.Claims;

namespace MyTasks.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
     
        private ITaskService _taskService;
        private ICategoryService _categoryService;


        public TaskController(ITaskService taskService, ICategoryService categoryService)
        {
         
            _taskService = taskService;
            _categoryService = categoryService;
        }
        public IActionResult Tasks()
        {
            var userId = User.GetUserId();

            var vm = new TasksViewModel
            {
                FilterTasks = new FilterTasks(),
                Tasks = _taskService.Get(userId),
                Categories = _categoryService.GetCategories(userId)
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Tasks(TasksViewModel viewModel)
        {
            var userId = User.GetUserId();
            var tasks = _taskService.Get(userId, viewModel.FilterTasks.IsExecuted,
                viewModel.FilterTasks.CategoryId, viewModel.FilterTasks.Title);
               


            return PartialView("_TasksTable", tasks);
        }

        public IActionResult Task(int id = 0)
        {
            var userId = User.GetUserId();

            var task = id == 0 ? new Core.Models.Domains.Task { Id = 0, UserId = userId, Term = DateTime.Today } : _taskService.Get(id, userId);

            var vm = new TaskViewModel
            {
                Task = task,
                Heading = id == 0 ?
                "Dodawanie nowego zadania" :
                "Edycja Zadania",
                Categories = _categoryService.GetCategories(userId)
            };
            return View(vm); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Task(Core.Models.Domains.Task task)
        {
            var userId = User.GetUserId();
            task.UserId = userId;
            if (!ModelState.IsValid)
            {
                var vm = new TaskViewModel
                {
                    Task = task,
                    Heading = task.Id == 0 ?
                "Dodawanie nowego zadania" :
                "Edycja Zadania",
                    Categories = _categoryService.GetCategories(userId)
                };
                return View("Task", vm);
            }

            if (task.Id == 0)
                _taskService.Add(task);
            else
                _taskService.Update(task);

            

            return RedirectToAction("Tasks");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _taskService.Delete(id, userId);
            
            }
            catch (Exception ex)
            {
                //logowanie do pliku

                return Json(new { success = false, message = ex.Message }); 
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Finish(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _taskService.Finish(id, userId);
               
            }
            catch (Exception ex)
            {
                //logowanie do pliku

                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = true });
        }
    }
}
