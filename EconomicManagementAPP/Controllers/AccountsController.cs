using AutoMapper;
using EconomicManagementAPP.Filters;
using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Controllers
{
    [TypeFilter(typeof(ExceptionManagerFilter))]
    public class AccountsController : Controller
    {
        private readonly IRepositorieAccountTypes repositorieAccountTypes;
        private readonly IUserServices serviceUser;
        private readonly IRepositorieAccounts repositorieAccounts;
        private readonly IMapper mapper;
        private readonly IRepositorieTransactions repositorieTransactions;

        public AccountsController(IRepositorieAccountTypes repositorieAccountTypes, IUserServices serviceUser,
            IRepositorieAccounts repositorieAccounts, IMapper mapper, IRepositorieTransactions repositorieTransactions)
        {
            this.repositorieAccountTypes = repositorieAccountTypes;
            this.serviceUser = serviceUser;
            this.repositorieAccounts = repositorieAccounts;
            this.mapper = mapper;
            this.repositorieTransactions = repositorieTransactions;
        }

        public async Task<IActionResult> Index()
        {
            var userId = serviceUser.GetUserId();
            var accountTypeExist = await repositorieAccounts.AccountList(userId);

            var model = accountTypeExist
                .GroupBy(x => x.AccountType)
                .Select(group => new AccountIndexViewModel
                {
                    AccountTypes = group.Key,
                    Accounts = group.AsEnumerable()
                }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Detail (int id, int month, int year)
        {
            var userId = serviceUser.GetUserId();
            var account = await repositorieAccounts.GetAccountById(id, userId);

            if(account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            DateTime startDate;
            DateTime endDate;

            if (month <= 0 || month > 12 || year <= 1900)
            {
                var today = DateTime.Today;
                startDate = new DateTime(today.Year, today.Month, 1);
            }
            else
            {
                startDate = new DateTime(year, month, 1);
            }

            endDate = startDate.AddMonths(1).AddDays(-1);

            var getTransactionsByAccount = new ParamGetTransactionsByAccount()
            {
                AccountId = id,
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate,
            };

            var transactions = await repositorieTransactions.GetByAccountId(getTransactionsByAccount);
            var model = new ReportTransactionsDetails();
            ViewBag.Account = account.Name;

            var transactionsByDate = transactions.OrderByDescending(x => x.TransactionDate)
                .GroupBy(x => x.TransactionDate)
                .Select(group => new ReportTransactionsDetails.TransactionsByDate()
                {
                    TransactionDate = group.Key,
                    Transactions = group.AsEnumerable()
                });

            model.TransactionsGrouped = transactionsByDate;
            model.StartDate = startDate;
            model.EndDate = endDate;

            ViewBag.previousMonth = startDate.AddMonths(-1).Month;
            ViewBag.previousYear = startDate.AddMonths(-1).Year;
            ViewBag.laterMonth = startDate.AddMonths(1).Month;
            ViewBag.laterYear = startDate.AddMonths(1).Year;
            ViewBag.urlReturn = HttpContext.Request.Path + HttpContext.Request.QueryString;

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = serviceUser.GetUserId();
            var model = new AccountViewModel();
            model.AccountTypes = await GetAccountTypes(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountViewModel account)
        {
            var userId = serviceUser.GetUserId();
            var accountType = await repositorieAccountTypes.GetAccountTypesById(account.AccountTypeId, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            if (!ModelState.IsValid)
            {
                account.AccountTypes = await GetAccountTypes(userId);
                return View(account);
            }

            await repositorieAccounts.Create(account);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Modify(int id)
        {
            var userId = serviceUser.GetUserId();
            var account = await repositorieAccounts.GetAccountById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var model = mapper.Map<AccountViewModel>(account); //Mapeo de manera automatica con todo el modelo sin ingresar manualmente los datos

            model.AccountTypes = await GetAccountTypes(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Modify(AccountViewModel accountModify)
        {
            var userId = serviceUser.GetUserId();
            var account = await repositorieAccounts.GetAccountById(accountModify.Id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var accountType = await repositorieAccountTypes.GetAccountTypesById(accountModify.AccountTypeId, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieAccounts.Modify(accountModify);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = serviceUser.GetUserId();
            var account = await repositorieAccounts.GetAccountById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = serviceUser.GetUserId();
            var account = await repositorieAccounts.GetAccountById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieAccounts.Delete(id);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> GetAccountTypes(int userId)
        {
            var accountTypes = await repositorieAccountTypes.GetAccountTypes(userId);
            return accountTypes.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }

    }
}
