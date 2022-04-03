using Dapper;
using EconomicManagementAPP.Models;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{

    public class RepositorieAccounts : IRepositorieAccounts
    {
        private readonly string connectionString;

        public RepositorieAccounts(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Account account)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO Accounts (Name, AccountTypeId, Balance, Description)
                    VALUES (@Name, @AccountTypeId, @Balance, @Description);
                    SELECT SCOPE_IDENTITY();", account);

            account.Id = id;
        }

        public async Task<IEnumerable<Account>> AccountList(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Account>(@"
                                    SELECT a.Id, a.Name, Balance, at.Name AS AccountType
                                    FROM Accounts AS a
                                    INNER JOIN AccountTypes AS at
                                    ON at.Id = a.AccountTypeId
                                    WHERE at.UserId = @UserId
                                    ORDER BY at.OrderAccount", new { userId });
        }

        public async Task<Account> GetAccountById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Account>(
                @"SELECT a.Id, a.Name, a.Balance, a.Description, AccountTypeId
                FROM Accounts AS a
                INNER JOIN AccountTypes AS at
                ON at.Id = a.AccountTypeId
                WHERE at.UserId = @UserId AND a.Id = @Id", new { id, userId });
        }

        public async Task Modify(AccountViewModel account)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Accounts
                                    SET Name = @Name, Balance = @Balance, Description = @Description,
                                    AccountTypeId = @AccountTypeId
                                    WHERE Id = @Id;", account);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Accounts WHERE Id = @Id", new { id });
        }
    }
}
