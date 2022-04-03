using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieAccountTypes
    {
        Task Create(AccountType accountTypes); // Se agrega task por el asincronismo
        Task<bool> Exist(string name, int userId);
        Task<IEnumerable<AccountType>> GetAccountTypes(int userId);
        Task Modify(AccountType accountTypes);
        Task<AccountType> GetAccountTypesById(int id, int userId); // para el modify
        Task Delete(int id);
        Task OrderAccount(IEnumerable<AccountType> accountTypesOrder); //para ordenar la lista de tipos de cuenta en el index
    }
}
