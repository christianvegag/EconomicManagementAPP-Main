using Dapper;
using EconomicManagementAPP.Models;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{

    public class RepositorieCategories : IRepositorieCategories
    {
        private readonly string connectionString;

        public RepositorieCategories(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Category category)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"
                                        INSERT INTO Categories (Name, OperationTypeId, UserId)
                                        Values (@Name, @OperationTypeId, @UserId);
                                        SELECT SCOPE_IDENTITY();
                                        ", category);

            category.Id = id;
        }

        public async Task<IEnumerable<Category>> GetCategories(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Category>(@"
                                       SELECT c.Id, c.Name, ot.Description AS OperationType, c.UserId FROM Categories AS c 
                                       JOIN OperationTypes AS ot ON ot.Id = c.OperationTypeId
                                       WHERE c.UserId = @UserId", new { userId });
        }

        public async Task<IEnumerable<Category>> GetCategoriesByOpType(int userId, int operationTypeId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Category>(@"
                                       SELECT c.Id, c.Name, ot.Description AS OperationType, c.UserId FROM Categories AS c 
                                       JOIN OperationTypes AS ot ON ot.Id = c.OperationTypeId
                                       WHERE c.UserId = @UserId AND c.OperationTypeId = @OperationTypeId", new { userId, operationTypeId });
        }


        public async Task<Category> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Category>(
                        @"Select * FROM Categories WHERE Id = @Id AND UserId = @UserId",
                        new { id, userId });
        }

        public async Task Modify(Category category)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Categories 
                    SET Name = @Name, OperationTypeId = @OperationTypeId
                    WHERE Id = @Id", category);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Categories WHERE Id = @Id", new { id });
        }
    }
}
