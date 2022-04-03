using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieOperationTypes
    {
        Task Create(OperationType operationType);
        Task Delete(int id);
        Task<bool> Exist(string description);
        Task<OperationType> GetOperationTypesById(int id);
        Task Modify(OperationType operationType);
        Task<IEnumerable<OperationType>> OperationTypesList();
    }
}
