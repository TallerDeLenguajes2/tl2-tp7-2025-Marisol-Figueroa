using Microsoft.Data.Sqlite;
using tl2_tp7_2025.Models;

namespace tl2_tp7_2025.Repositorios
{
    public class ProductoRepository
    {
        string cadenaConexion = "Data Source=Tienda_final.db";

        // Listar todos los Productos registrados.
        public List<Producto> GetAll()
        {
            var lista = new List<Producto>();
            using (var conexion = new SqliteConnection(cadenaConexion))
            {
                conexion.Open();
                var command = conexion.CreateCommand();
                command.CommandText = "SELECT idProducto, descripcion, precio FROM Productos;";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Producto
                        {
                            idProducto = reader.GetInt32(0),
                            descripcion = reader.GetString(1),
                            precio = reader.GetInt32(2)
                        });
                    }
                }
            }
            return lista;
        }

        // Crear un nuevo Producto.
        public void Crear(Producto p)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            string sql = "INSERT INTO Productos (descripcion, precio) VALUES (@descripcion, @precio);";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@descripcion", p.descripcion);
            comando.Parameters.AddWithValue("@precio", p.precio);
            comando.ExecuteNonQuery();
        }

        // Obtener un Producto por su ID.
        public Producto GetById(int id)
        {
            Producto producto = null;
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            string sql = "SELECT idProducto, descripcion, precio FROM Productos WHERE idProducto = @id;";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@id", id);
            using var reader = comando.ExecuteReader();
            if (reader.Read())
            {
                producto = new Producto
                {
                    idProducto = reader.GetInt32(0),
                    descripcion = reader.GetString(1),
                    precio = reader.GetInt32(2)
                };
            }
            return producto;
        }

        // Modificar un Producto existente.
        public void ActualizarProducto(int id, Producto producto)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            string sql = "UPDATE Productos SET descripcion = @descripcion, precio = @precio WHERE idProducto = @idProducto;";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@idProducto", id);
            comando.Parameters.AddWithValue("@descripcion", producto.descripcion);
            comando.Parameters.AddWithValue("@precio", producto.precio);
            comando.ExecuteNonQuery();
        }

        // Eliminar un Producto por ID.
        public bool Eliminar(int id)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            string sql = "DELETE FROM Productos WHERE idProducto = @idProducto;";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@idProducto", id);
            int filasAfectadas = comando.ExecuteNonQuery();
            return filasAfectadas > 0; // true si eliminó algo
            
        }
    }
}
