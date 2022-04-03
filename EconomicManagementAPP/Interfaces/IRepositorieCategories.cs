using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieCategories
    {
        Task Modify(Category category);
        Task Delete(int id);
        Task Create(Category category);
        Task<IEnumerable<Category>> GetCategories(int userId);
        Task<Category> GetById(int id, int userId);
        Task<IEnumerable<Category>> GetCategoriesByOpType(int userId, int operationTypeId);
    }
}
