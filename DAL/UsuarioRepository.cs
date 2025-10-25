using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOM;
using ABS.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddAsync(Usuario usuario)
        {
            const string query = @"
                INSERT INTO Usuario (Nombre, Apellido, Email, Clave, Estado, Rol, FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificador)
                VALUES (@Nombre, @Apellido, @Email, @Clave, @Estado, @Rol, @FechaCreacion, @UsuarioCreacion, @FechaModificacion, @UsuarioModificador);
                SELECT SCOPE_IDENTITY();"; // Esta linea devuelve el ID del registro insertado

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Nombre", usuario.Nombre ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Apellido", usuario.Apellido ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", usuario.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Clave", usuario.Apellido ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Estado", usuario.Apellido);
                command.Parameters.AddWithValue("@Rol", usuario.Rol);
                command.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                command.Parameters.AddWithValue("@UsuarioCreacion", Environment.UserName);
                command.Parameters.AddWithValue("@FechaModificacion", (object)DBNull.Value); // Es NULL en alta
                command.Parameters.AddWithValue("@UsuarioModificador", (object)DBNull.Value); // Lo mismo que arriba



                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();

                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al dar de alta el usuario", ex);
            }
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            const string query = @"
                SELECT ID, Nombre, Apellido, Email, Clave, Estado, Rol, FechaCreacion, UsuarioCreacion,  UsuarioModificador, FechaModificacion
                FROM Usuario WHERE ID = @ID";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return MapFromReader(reader);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario por ID", ex);
            }
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            const string query = @"
                SELECT ID, Nombre, Apellido, Email, Estado
                FROM Usuario
                ORDER BY ID";

            var usuarios = new List<Usuario>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(query, connection);

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    usuarios.Add(new Usuario
                    {
                        ID = reader.GetInt32("ID"),
                        Apellido = reader.IsDBNull("Apellido") ? null : reader.GetString("Apellido"),
                        Nombre = reader.IsDBNull("Nombre") ? null : reader.GetString("Nombre"),
                        Estado = reader.GetInt32("Estado")
                    });

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de usuarios", ex);
            }

            return usuarios;
        }

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            const string query = @"
                UPDATE Usuario SET
                    Nombre = @Nombre,
                    Apellido = @Apellido,
                    Email = @Email,
                    Clave = @Clave,
                    Estado = @Estado,
                    Rol = @Rol,
                    FechaModificacion = @FechaModificacion,
                    UsuarioModificador = @UsuarioModificador,
                WHERE ID = @ID
                ";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", usuario.ID);
                command.Parameters.AddWithValue("@Nombre", usuario.Nombre ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Apellido", usuario.Apellido ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", usuario.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Clave", usuario.Clave ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Estado", usuario.Estado);
                command.Parameters.AddWithValue("@Rol", usuario.Rol);
                command.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                command.Parameters.AddWithValue("@UsuarioModificador", Environment.UserName);

                await connection.OpenAsync();
                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el usuario", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string query = "UPDATE Usuario SET Estado = 1 WHERE ID = @ID"; // Estado = 1 es baja lógica

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);

                await connection.OpenAsync();
                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;

            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un error al dar de baja el usuario", ex);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            const string query = "SELECT COUNT(1) FROM Usuario WHERE ID = @ID AND Estado = 0"; // Solo activos

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);

                await connection.OpenAsync();
                object result = command.ExecuteScalarAsync();
                return Convert.ToInt32(result) > 0;

            } 
            catch (Exception ex)
            {
                throw new Exception("Hubo un error al verificar si el usuario existe", ex);
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            const string query = "SELECT COUNT(1) FROM Usuario WHERE Email = @Email AND Estado = 0"; // Solo activos

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                await connection.OpenAsync();
                object result = command.ExecuteScalarAsync();
                return Convert.ToInt32(result) > 0;
            }
            catch(Exception ex)
            {
                throw new Exception("Error al verificar si el email ya existe", ex);
            }
        }

        private Usuario MapFromReader(SqlDataReader reader)
        {
            return new Usuario
            {
                ID = reader.GetInt32("ID"),
                Nombre = reader.IsDBNull("Nombre") ? null : reader.GetString("Nombre"),
                Apellido = reader.IsDBNull("Apellido") ? null : reader.GetString("Apellido"),
                Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                Clave = reader.IsDBNull("Clave") ? null : reader.GetString("Clave"),
                Estado = reader.GetInt32("Estado"),
                Rol = reader.GetInt32("Rol"),
                FechaCreacion = reader.GetDateTime("FechaAlta"),
                UsuarioCreacion = reader.IsDBNull("UsuarioAlta") ? null : reader.GetString("UsuarioAlta"),
                FechaModificacion = reader.IsDBNull("FechaModificacion") ? null : reader.GetDateTime("FechaModificacion"),
                UsuarioModificador = reader.IsDBNull("UsuarioModificador") ? null : reader.GetString("UsuarioModificador")

            };
        }   
    }
}
