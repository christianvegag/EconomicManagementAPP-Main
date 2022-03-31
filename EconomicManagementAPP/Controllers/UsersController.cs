using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class UsersController : Controller
    {
        private readonly IRepositorieUsers repositorieUsers;

        public UsersController(IRepositorieUsers repositorieUsers)
        {
            this.repositorieUsers = repositorieUsers;
        }   

        public async Task<IActionResult> Index()
        {
            var users = await repositorieUsers.GetUsers();
            return View(users);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            if(user.Email == user.StandarEmail) {
                ModelState.AddModelError(nameof(user.StandarEmail),
                    $"Email cannot be repeated ");
                return View(user);
            }

            var userExist =
               await repositorieUsers.Exist(user.Email, user.StandarEmail);

            if (userExist)
            {
                ModelState.AddModelError(nameof(user.Email),
                    $"Any Email already use.");

                return View(user);
            }
            await repositorieUsers.Create(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificaryUser(string Email, string StandarEmail)
        {
            var userExist = await repositorieUsers.Exist(Email, StandarEmail);

            if (userExist)
            {
                return Json($"The account {Email}{StandarEmail} already exist");
            }

            return Json(true);
        }

        //Actualizar
        [HttpGet]
        public async Task<ActionResult> Modify(int id)
        {
            var user = await repositorieUsers.GetUserById(id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> Modify(User user)
        {
            var userExists = await repositorieUsers.GetUserById(user.Id);

            if (userExists is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieUsers.Modify(user);
            return RedirectToAction("Index");
        }
        // Eliminar
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await repositorieUsers.GetUserById(id);

            if (user is null)
            {
                return RedirectToAction("NotFount", "Home");
            }

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await repositorieUsers.GetUserById(id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieUsers.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
