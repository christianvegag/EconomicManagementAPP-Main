using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieUsers
    {
        Task Create(User user); // Se agrega task por el asincronismo
        Task<bool> Exist(string email, string standarEmail);
        Task<IEnumerable<User>> GetUsers();
        Task Modify(User user);
        Task<User> GetUserById(int id); // para el modify
        Task Delete(int id);
    }
}
