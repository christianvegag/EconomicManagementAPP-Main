using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieUsers
    {
        Task<int> Create(User user);
        Task<User> FindUserByEmail(string standarEmail);
    }
}
