using EconomicManagementAPP.Filters;
using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    [TypeFilter(typeof(ExceptionManagerFilter))]

    public class OperationTypesController : Controller
    {
        private readonly IRepositorieOperationTypes repositorieOperationTypes;

        public OperationTypesController(IRepositorieOperationTypes repositorieOperationTypes)
        {
            this.repositorieOperationTypes = repositorieOperationTypes;
        }

        public async Task<IActionResult> Index()
        {
            // Simula que estamos logeados en la app.
            var operationTypes = await repositorieOperationTypes.OperationTypesList();
            return View(operationTypes);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OperationType operationType)
        {
            if (!ModelState.IsValid)
            {
                return View(operationType);
            }

            // Validamos si ya existe antes de registrar
            var operationTypeExist =
               await repositorieOperationTypes.Exist(operationType.Description);

            if (operationTypeExist)
            {
                ModelState.AddModelError(nameof(operationType.Description),
                    $"The OperationType {operationType.Description} already exist.");

                return View(operationType);
            }
            await repositorieOperationTypes.Create(operationType);
            return RedirectToAction("Index");
        }

        // Hace que la validacion se active automaticamente desde el front
        [HttpGet]
        public async Task<IActionResult> VerificaryOperationType(string Description)
        {
            var accountTypeExist = await repositorieOperationTypes.Exist(Description);

            if (accountTypeExist)
            {
                // permite acciones directas entre front y back
                return Json($"The account {Description} already exist");
            }

            return Json(true);
        }
        
        //Actualizar
        [HttpGet]
        public async Task<ActionResult> Modify(int id)
        {
            var operationType = await repositorieOperationTypes.GetOperationTypesById(id);

            if (operationType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(operationType);
        }
        [HttpPost]
        public async Task<ActionResult> Modify(OperationType operationType)
        {
            var accountTypeExist = await repositorieOperationTypes.GetOperationTypesById(operationType.Id);

            if (accountTypeExist is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieOperationTypes.Modify(operationType);
            return RedirectToAction("Index");
        }
        // Eliminar
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var account = await repositorieOperationTypes.GetOperationTypesById(id);

            if (account is null)
            {
                return RedirectToAction("NotFount", "Home");
            }

            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteOperationTypes(int id)
        {
            var account = await repositorieOperationTypes.GetOperationTypesById(id);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieOperationTypes.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
