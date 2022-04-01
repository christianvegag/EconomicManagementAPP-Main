using Dapper;
using EconomicManagementAPP.Models;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieOperationTypes
    {
        Task Create(OperationType operationType);
        Task Delete(int id);
        Task<bool> Exist(string description);
        Task<OperationType> getOperationTypesById(int id);
        Task Modify(OperationType operationType);
        Task<IEnumerable<OperationType>> OperationTypesList();
    }
    public class RepositorieOperationTypes : IRepositorieOperationTypes
    {
        private readonly string connectionString;

        public RepositorieOperationTypes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(OperationType operationType)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                                    @"INSERT INTO OperationTypes (Description)
                                                    VALUES (@Description);
                                                    SELECT SCOPE_IDENTITY();", operationType);

            operationType.Id = id;
        }
        public async Task<bool> Exist(string description)
        {
            using var connection = new SqlConnection(connectionString);

            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1
                                    FROM OperationTypes
                                    WHERE Description = @Description",
                                    new { description });
            return exist == 1;
        }

        // Obtenemos los tipos
        public async Task<IEnumerable<OperationType>> OperationTypesList()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<OperationType>(@"SELECT Id, Description
                                                            FROM OperationTypes
                                                            ORDER BY Id");
        }
        // Actualizar
        public async Task Modify(OperationType operationType)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE OperationTypes
                                            SET Description = @Description
                                            WHERE Id = @Id", operationType);
        }

        public async Task<OperationType> getOperationTypesById(int id)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<OperationType>(@"
                                                                SELECT Id, Description
                                                                FROM OperationTypes
                                                                WHERE Id = @Id",
                                                                new { id });
        }

        //Eliminar
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE OperationTypes WHERE Id = @Id", new { id });
        }

    }
}
