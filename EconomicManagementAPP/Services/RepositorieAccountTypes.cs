using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public class RepositorieAccountTypes : IRepositorieAccountTypes
    {
        private readonly string connectionString;

        public RepositorieAccountTypes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // El async va acompañado de Task
        public async Task Create(AccountType accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
            // Requiere el await - tambien requiere el Async al final de la query
            var id = await connection.QuerySingleAsync<int>
                                                            ("SP_AccountType_Insert",
                                                            new
                                                            {
                                                                Name = accountTypes.Name,
                                                                UserId = accountTypes.UserId
                                                            },
                                                        commandType: System.Data.CommandType.StoredProcedure);
            accountTypes.Id = id;
        }

        //Cuando retorna un tipo de dato se debe poner en el Task Task<bool>
        // new { Name, UserId } -> según las consultas aplicando dapper nos dice que se sigue un orden (sql, object param)
        // por lo que nos pide un objeto a ser Name & UserId dos datos fuera de un objeto son convertidos para así poder realizar las consultas.
        public async Task<bool> Exist(string name, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            // El select 1 es traer lo primero que encuentre y el default es 0
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1
                                    FROM AccountTypes
                                    WHERE Name = @Name AND UserId = @UserId;",
                                    new { name, userId });
            return exist == 1;
        }

        // Obtenemos las cuentas del usuario
        public async Task<IEnumerable<AccountType>> GetAccountTypes(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountType>(@"SELECT Id, Name, OrderAccount
                                                            FROM AccountTypes
                                                            WHERE UserId = @UserId
                                                            ORDER BY OrderAccount", new { userId });
        }
        // Actualizar
        public async Task Modify(AccountType accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE AccountTypes
                                            SET Name = @Name
                                            WHERE Id = @Id", accountTypes);
        }

        //Para actualizar se necesita obtener el tipo de cuenta por el id
        public async Task<AccountType> GetAccountTypesById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<AccountType>(@"
                                                                SELECT Id, Name, UserId, OrderAccount
                                                                FROM AccountTypes
                                                                WHERE Id = @Id AND UserID = @UserID",
                                                                new { id, userId });
        }

        //Eliminar
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE AccountTypes WHERE Id = @Id", new { id });
        }

        //Ordenar por tipo de cuentas
        public async Task OrderAccount(IEnumerable<AccountType> accountTypesOrder)
        {
            var query = "UPDATE accountTypes SET OrderAccount = @OrderAccount Where Id = @Id;";
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, accountTypesOrder);
        }
    }
}
