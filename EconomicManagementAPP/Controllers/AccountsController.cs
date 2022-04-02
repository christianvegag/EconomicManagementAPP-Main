using AutoMapper;
using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IRepositorieAccountTypes repositorieAccountTypes;
        private readonly IServiceUser serviceUser;
        private readonly IRepositorieAccounts repositorieAccounts;
        private readonly IMapper mapper;

        public AccountsController(IRepositorieAccountTypes repositorieAccountTypes, IServiceUser serviceUser,
            IRepositorieAccounts repositorieAccounts, IMapper mapper)
        {
            this.repositorieAccountTypes = repositorieAccountTypes;
            this.serviceUser = serviceUser;
            this.repositorieAccounts = repositorieAccounts;
            this.mapper = mapper;
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = serviceUser.GetUserId();
            var model = new AccountCreateViewModel();
            model.AccountTypes = await GetAccountTypes(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountCreateViewModel account)
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

            var model = mapper.Map<AccountCreateViewModel>(account); //Mapeo de manera automatica con todo el modelo sin ingresar manualmente los datos

            model.AccountTypes = await GetAccountTypes(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Modify(AccountCreateViewModel accountModify)
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
