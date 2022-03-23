using Dapper;
using EconomicManagementAPP.Models;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieUsers
    {
        Task Create(Users user); // Se agrega task por el asincronismo
        Task<bool> Exist(string Email, string StandarEmail);
        Task<IEnumerable<Users>> GetUsers();
        Task Modify(Users user);
        Task<Users> GetUserById(int id); // para el modify
        Task Delete(int id);
    }
    public class RepositorieUsers : IRepositorieUsers
    {
        private readonly string connectionString;

        public RepositorieUsers(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(Users user)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Users 
                                                (Email, StandarEmail, Password) 
                                                VALUES (@Email, @StandarEmail, @Password); SELECT SCOPE_IDENTITY();", user);
            user.Id = id;
        }

        public async Task<bool> Exist(string Email, string StandarEmail)
        {
            using var connection = new SqlConnection(connectionString);
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1
                                    FROM Users
                                    WHERE Email IN (@Email,@StandarEmail) OR StandarEmail IN (@Email,@StandarEmail);",
                                    new { Email, StandarEmail });
            return exist == 1;
        }

        public async Task<IEnumerable<Users>> GetUsers()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Users>("SELECT Id, Email, StandarEmail FROM Users;");
        }
        public async Task Modify(Users user)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Users SET 
                                            Password = @Password,
                                            StandarEmail = @StandarEmail
                                            WHERE Id = @Id", user);
        }

        public async Task<Users> GetUserById(int id)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Users>(@"SELECT Id, Email, StandarEmail, Password
                                                                FROM Users
                                                                WHERE Id = @Id",
                                                                new { id });
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Users WHERE Id = @Id", new { id });
        }
    }
}
