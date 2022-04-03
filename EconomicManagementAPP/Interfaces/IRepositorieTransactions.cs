using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieTransactions
    {
        Task Create(Transaction transaction);
        Task Delete(int id);
        Task<IEnumerable<Transaction>> GetByAccountId(ParamGetTransactionsByAccount model);
        Task<IEnumerable<Transaction>> GetByUserId(ParamGetTransactionsByUser model);
        Task<Transaction> GetTransactionById(int id, int userId);
        Task Modify(Transaction transaction, decimal previusTotal, int previusAccount);
    }
}
