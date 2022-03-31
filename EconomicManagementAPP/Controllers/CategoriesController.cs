using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IRepositorieCategories repositorieCategories;
        private readonly IServiceUser serviceUser;

        public CategoriesController(IRepositorieCategories repositorieCategories,
            IServiceUser serviceUser)
        {
            this.repositorieCategories = repositorieCategories;
            this.serviceUser = serviceUser;
        }

        public async Task<IActionResult> Index()
        {
            var userId = serviceUser.GetUserId();
            var categories = await repositorieCategories.GetCategories(userId);
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var userId = serviceUser.GetUserId();
            category.UserId = userId;
            await repositorieCategories.Create(category);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Modify(int id)
        {
            var userId = serviceUser.GetUserId();
            var category = await repositorieCategories.GetById(id, userId);

            if (category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Modify(Category categoryModify)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryModify);
            }

            var userId = serviceUser.GetUserId();
            var category = await repositorieCategories.GetById(categoryModify.Id, userId);

            if (category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            categoryModify.UserId = userId;
            await repositorieCategories.Modify(categoryModify);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = serviceUser.GetUserId();
            var category = await repositorieCategories.GetById(id, userId);

            if (category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = serviceUser.GetUserId();
            var category = await repositorieCategories.GetById(id, userId);

            if (category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieCategories.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
