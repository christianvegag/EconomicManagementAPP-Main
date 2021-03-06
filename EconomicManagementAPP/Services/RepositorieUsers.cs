using Dapper;
using EconomicManagementAPP.Models;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public class RepositorieUsers : IRepositorieUsers
    {
        private readonly string connectionString;

        public RepositorieUsers(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> Create(User user)
        {
            using var connection = new SqlConnection(connectionString);
            var userId = await connection.QuerySingleAsync<int>(@"INSERT INTO Users 
                                                (Email, StandarEmail, Password) 
                                                VALUES (@Email, @StandarEmail, @Password);
                                                SELECT SCOPE_IDENTITY();", user);

            await connection.ExecuteAsync("SP_CreateDataUserNew", new { userId }, commandType: System.Data.CommandType.StoredProcedure);
            return userId;
        }

        public async Task<User> FindUserByEmail(string standarEmail)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE StandarEmail = @standarEmail",
                new { standarEmail });
        }


        //public async Task<bool> Exist(string email, string standarEmail)
        //{
        //    using var connection = new SqlConnection(connectionString);
        //    var exist = await connection.QueryFirstOrDefaultAsync<int>(
        //                            @"SELECT 1
        //                            FROM Users
        //                            WHERE Email IN (@Email,@StandarEmail) OR StandarEmail IN (@Email,@StandarEmail);",
        //                            new { email, standarEmail });
        //    return exist == 1;
        //}

        //public async Task<IEnumerable<User>> GetUsers()
        //{
        //    using var connection = new SqlConnection(connectionString);
        //    return await connection.QueryAsync<User>("SELECT Id, Email, StandarEmail FROM Users;");
        //}
        //public async Task Modify(User user)
        //{
        //    using var connection = new SqlConnection(connectionString);
        //    await connection.ExecuteAsync(@"UPDATE Users SET 
        //                                    Password = @Password,
        //                                    StandarEmail = @StandarEmail
        //                                    WHERE Id = @Id", user);
        //}

        //public async Task<User> GetUserById(int id)
        //{
        //    using var connection = new SqlConnection(connectionString);

        //    return await connection.QueryFirstOrDefaultAsync<User>(@"SELECT Id, Email, StandarEmail, Password
        //                                                        FROM Users
        //                                                        WHERE Id = @Id",
        //                                                        new { id });
        //}

        //public async Task Delete(int id)
        //{
        //    using var connection = new SqlConnection(connectionString);
        //    await connection.ExecuteAsync("DELETE Users WHERE Id = @Id", new { id });
        //}
    }
}
