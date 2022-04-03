using Dapper;
using EconomicManagementAPP.Models;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public class RepositorieTransactions : IRepositorieTransactions
    {
        private readonly string connectionString;
        public RepositorieTransactions(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(Transaction transaction)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"SP_Transactions_Insert",
                                                    new
                                                    {
                                                        transaction.UserId,
                                                        transaction.TransactionDate,
                                                        transaction.Total,
                                                        transaction.Description,
                                                        transaction.AccountId,
                                                        transaction.CategoryId
                                                    }, commandType: System.Data.CommandType.StoredProcedure);
            transaction.Id = id;
        }

        public async Task<Transaction> GetTransactionById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Transaction>(@"SELECT t.*, c.OperationTypeId
                                                                           FROM Transactions AS t
                                                                           JOIN Categories AS c ON c.Id = t.CategoryId
                                                                           WHERE t.Id = @Id AND t.UserId = @UserId",
                                                                           new { id, userId });
        }

        //Obtener Cuentas por ID
        public async Task <IEnumerable<Transaction>> GetByAccountId (ParamGetTransactionsByAccount model)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaction>(@"
                                SELECT t.Id, t.Total, t.TransactionDate, c.Name AS Category,
                                a.Name AS Account, c.OperationTypeId, ot.Description AS OperationType
                                FROM Transactions AS t
                                JOIN Categories AS c ON c.Id = t.CategoryId
                                JOIN Accounts AS a ON a.Id = t.AccountId
                                JOIN OperationTypes AS ot ON ot.Id = c.OperationTypeId
                                WHERE t.AccountId = @AccountId AND t.UserId = @UserId
                                AND TransactionDate BETWEEN @StartDate AND @EndDate", model);
        }


        //// Obtener Transacciones de una cuenta
        //public async Task<IEnumerable<Transaction>> GetTransactions(int accountId, int userId)
        //{
        //    using var connection = new SqlConnection(connectionString);
        //    return await connection.QueryAsync<Transaction>(@"SELECT t.Id, t.UserId, t.TransactionDate, t.Total, t.OperationTypeId, t.Description, t.AccountId, t.CategoryId, c.Name AS Category
        //                                                     FROM Transactions t
        //                                                     JOIN Categories c ON c.Id = t.CategoryId
        //                                                     WHERE t.AccountId = @accountId AND t.UserId = @userId
        //                                                     ORDER BY t.Id DESC;", new { accountId, userId });
        //}

        public async Task Modify(Transaction transaction, decimal previusTotal, int previusAccountId)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("SP_Transactions_Update",
                new
                {
                    transaction.Id,
                    transaction.TransactionDate,
                    transaction.Total,
                    transaction.Description,
                    transaction.AccountId,
                    transaction.CategoryId,
                    previusTotal,
                    previusAccountId
                }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("SP_Transactions_Delete",
                 new { id }, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
