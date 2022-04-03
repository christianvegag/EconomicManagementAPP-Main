using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieAccounts
    {
        Task Modify(AccountViewModel account);
        Task Delete(int id);
        Task<IEnumerable<Account>> AccountList(int userId);
        Task Create(Account account);
        Task<Account> GetAccountById(int id, int userId);
    }
}
