using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class AccountTypesController : Controller
    {
        private readonly IRepositorieAccountTypes repositorieAccountTypes;
        private readonly IServiceUser serviceUser;
        public AccountTypesController(IRepositorieAccountTypes repositorieAccountTypes, IServiceUser serviceUser)
        {
            this.repositorieAccountTypes = repositorieAccountTypes;
            this.serviceUser = serviceUser;
        }

        // Creamos index para ejecutar la interfaz
        // Task : Representa una operación asincrónica que puede devolver un valor.
        public async Task<IActionResult> Index()
        {
            // Simula que estamos logeados en la app.
            var userId = serviceUser.GetUserId();
            var accountTypes = await repositorieAccountTypes.getAccounts(userId);
            return View(accountTypes);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountType accountType)
        {
            if (!ModelState.IsValid)
            {
                return View(accountType);
            }

            accountType.UserId = serviceUser.GetUserId();
            // Validamos si ya existe antes de registrar
            var accountTypeExist =
               await repositorieAccountTypes.Exist(accountType.Name, accountType.UserId);

            if (accountTypeExist)
            {
                // AddModelError ya viene predefinido en .net
                // nameOf es el tipo del campo
                ModelState.AddModelError(nameof(accountType.Name),
                    $"The account {accountType.Name} already exist.");

                return View(accountType);
            }
            await repositorieAccountTypes.Create(accountType);
            // Redireccionamos a la lista
            return RedirectToAction("Index");
        }

        // Hace que la validacion se active automaticamente desde el front
        [HttpGet]
        public async Task<IActionResult> VerificaryAccountType(string Name)
        {
            var UserId = serviceUser.GetUserId();
            var accountTypeExist = await repositorieAccountTypes.Exist(Name, UserId);

            if (accountTypeExist)
            {
                // permite acciones directas entre front y back
                return Json($"The account {Name} already exist");
            }

            return Json(true);
        }

        //Actualizar
        [HttpGet]
        public async Task<ActionResult> Modify(int id)
        {
            var userId = serviceUser.GetUserId();

            var accountType = await repositorieAccountTypes.getAccountById(id, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(accountType);
        }
        [HttpPost]
        public async Task<ActionResult> Modify(AccountType accountType)
        {
            var userId = serviceUser.GetUserId();
            var accountTypeExist = await repositorieAccountTypes.getAccountById(accountType.Id, userId);

            if (accountTypeExist is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieAccountTypes.Modify(accountType);// el que llega
            return RedirectToAction("Index");
        }
        // Eliminar
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = serviceUser.GetUserId();
            var account = await repositorieAccountTypes.getAccountById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFount", "Home");
            }

            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = serviceUser.GetUserId();
            var account = await repositorieAccountTypes.getAccountById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieAccountTypes.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> OrderAccount([FromBody] int[] ids)
        {
            var userId = serviceUser.GetUserId();
            var accountType = await repositorieAccountTypes.getAccounts(userId);
            var idsAccountType = accountType.Select(x => x.Id);

            var idsTypeAccountNotUser = ids.Except(idsAccountType).ToList();

            if (idsTypeAccountNotUser.Count > 0)
            {
                return Forbid();
            }

            var typeAccountOrder = ids.Select((valor, index) =>
                new AccountType() { Id = valor, OrderAccount = index + 1 }).AsEnumerable();

            await repositorieAccountTypes.OrderAccount(typeAccountOrder);

            return Ok();
        }
    }
}
