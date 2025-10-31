using Microsoft.Data.Sqlite;
using tl2_tp7_2025_TuNombre.Models;

namespace tl2_tp7_2025_TuNombre.Repositorios;

public class ProductosRepository
{
    private string connectionString = "Data Source=Tienda_final.db";

    public List<Producto> GetAll()
    {
        var lista = new List<Producto>();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT idProducto, Descripcion, Precio FROM Productos;";
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

    public void Alta(Producto p)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Productos (Descripcion, Precio) VALUES ($desc, $precio)";
            command.Parameters.AddWithValue("$desc", p.descripcion);
            command.Parameters.AddWithValue("$precio", p.precio);
            command.ExecuteNonQuery();
        }
    }

    public bool Eliminar(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Productos WHERE idProducto = $id";
            command.Parameters.AddWithValue("$id", id);
            return command.ExecuteNonQuery() > 0;
        }
    }
}
