using ABS.Interfaces;
using DOM;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public class RolRepository : IRolRepository
    {
        private readonly string _connectionString;

        public RolRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            var roles = new List<Rol>();
            const string query = "SELECT ID, Detalle FROM Rol";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(query, connection);

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    roles.Add(new Rol
                    {
                        ID = reader.GetInt32(0),
                        Detalle = reader.GetString(1)
                    });
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception($"Error al obtener los roles: {ex.Message}", ex);
            }

            return roles;
        }
    }
}
