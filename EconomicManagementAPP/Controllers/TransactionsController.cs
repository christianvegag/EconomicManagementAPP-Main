using AutoMapper;
using EconomicManagementAPP.Filters;
using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Controllers
{
    [TypeFilter(typeof(ExceptionManagerFilter))]

    public class TransactionsController : Controller
    {
        private readonly IRepositorieTransactions repositorieTransactions;
        private readonly IRepositorieAccounts repositorieAccounts;
        private readonly IRepositorieCategories repositorieCategories;
        private readonly IRepositorieOperationTypes repositorieOperationTypes;
        private readonly IUserServices userServices;
        private readonly IMapper mapper;
        private readonly IReportServices reportServices;

        public TransactionsController(IRepositorieTransactions repositorieTransactions,
                                      IRepositorieAccounts repositorieAccounts,
                                      IRepositorieCategories repositorieCategories,
                                      IRepositorieOperationTypes repositorieOperationTypes,
                                      IUserServices userServices,
                                      IMapper mapper,
                                      IReportServices reportServices)
        {
            this.repositorieTransactions = repositorieTransactions;
            this.repositorieAccounts = repositorieAccounts;
            this.repositorieCategories = repositorieCategories;
            this.repositorieOperationTypes = repositorieOperationTypes;
            this.userServices = userServices;
            this.mapper = mapper;
            this.reportServices = reportServices;
        }

        public async Task<IActionResult> Index(int month, int year)
        {
            var userId = userServices.GetUserId();
            var model = await reportServices.GetReportTransactionsDetailed(userId, month, year, ViewBag);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = userServices.GetUserId();
            var model = new TransactionViewModel();
            model.Accounts = await GetAccounts(userId);
            model.OperationTypes = await GetOperationTypes();
            model.Categories = await GetCategories(userId, model.OperationTypeId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransactionViewModel model)
        {
            var userId = userServices.GetUserId();

            if (!ModelState.IsValid)
            {
                model.Accounts = await GetAccounts(userId);
                model.OperationTypes = await GetOperationTypes();
                model.Categories = await GetCategories(userId, model.OperationTypeId);
                return View(model);
            }

            var account = await repositorieAccounts.GetAccountById(model.AccountId, userId);
            var category = await repositorieCategories.GetById(model.CategoryId, userId);

            if (account is null || category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            model.UserId = userId;

            var operationType = await repositorieOperationTypes.GetOperationTypesById(model.OperationTypeId);

            if (operationType.Description == "Expense")
            {
                model.Total *= -1;
            }

            await repositorieTransactions.Create(model);
            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        public async Task<ActionResult> Modify(int id, string urlReturn = null)
        {
            var userId = userServices.GetUserId();
            var transaction = await repositorieTransactions.GetTransactionById(id, userId);

            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var model = mapper.Map<TransactionUpdateViewModel>(transaction);

            model.PreviousTotal = model.Total;

            var operationType = await repositorieOperationTypes.GetOperationTypesById(model.OperationTypeId);

            if (operationType.Description == "Expense")
            {
                model.PreviousTotal = model.Total *= -1;
            }

            model.PreviousAccountId = transaction.AccountId;
            model.OperationTypes = await GetOperationTypes();
            model.Categories = await GetCategories(userId, transaction.OperationTypeId);
            model.Accounts = await GetAccounts(userId);
            model.UrlReturn = urlReturn;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Modify(TransactionUpdateViewModel model)
        {
            var userId = userServices.GetUserId();

            if (!ModelState.IsValid)
            {
                model.Accounts = await GetAccounts(userId);
                model.OperationTypes = await GetOperationTypes();
                model.Categories = await GetCategories(userId, model.OperationTypeId);
                return View(model);
            }


            var account = await repositorieAccounts.GetAccountById(model.AccountId, userId);
            var category = await repositorieCategories.GetById(model.CategoryId, userId);

            if (account is null || category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var transaction = mapper.Map<Transaction>(model);

            var operationType = await repositorieOperationTypes.GetOperationTypesById(model.OperationTypeId);

            if (operationType.Description == "Expense")
            {
                transaction.Total *= -1;
            }

            await repositorieTransactions.Modify(transaction, model.PreviousTotal, model.PreviousAccountId);

            if (string.IsNullOrEmpty(model.UrlReturn))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return LocalRedirect(model.UrlReturn);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id, string urlReturn = null)
        {
            var userId = userServices.GetUserId();
            var transaction = await repositorieTransactions.GetTransactionById(id, userId);

            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieTransactions.Delete(id);

            if (string.IsNullOrEmpty(urlReturn))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return LocalRedirect(urlReturn);
            }
        }



        private async Task<IEnumerable<SelectListItem>> GetAccounts(int userId)
        {
            var accounts = await repositorieAccounts.AccountList(userId);
            return accounts.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }
        private async Task<IEnumerable<SelectListItem>> GetOperationTypes()
        {
            var OperationTypes = await repositorieOperationTypes.OperationTypesList();
            return OperationTypes.Select(x => new SelectListItem(x.Description, x.Id.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> GetCategories(int userId, int operationTypeId)
        {
            var categories = await repositorieCategories.GetCategoriesByOpType(userId, operationTypeId);
            return categories.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> GetCategories([FromBody] int operationTypeId)
        {
            var userId = userServices.GetUserId();
            var categories = await GetCategories(userId, operationTypeId);
            return Ok(categories);
        }




    }
}
