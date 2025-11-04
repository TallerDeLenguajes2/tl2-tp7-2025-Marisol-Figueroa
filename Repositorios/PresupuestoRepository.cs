using Microsoft.Data.Sqlite;
using tl2_tp7_2025.Models;

namespace tl2_tp7_2025.Repositorios
{
    public class PresupuestoRepository
    {
        string cadenaConexion = "Data Source=Tienda_final.db";

        // Crear un nuevo Presupuesto.
        public void CrearPresupuesto(Presupuesto presupuesto)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            string sql = "INSERT INTO Presupuestos (nombreDestinatario, FechaCreacion) VALUES (@nombreDestinatario, @FechaCreacion);";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@nombreDestinatario", presupuesto.nombreDestinatario);
            comando.Parameters.AddWithValue("@FechaCreacion", presupuesto.FechaCreacion);
            comando.ExecuteNonQuery();
        }

        // Listar todos los Presupuestos.
        public List<Presupuesto> GetAll()
        {
            var lista = new List<Presupuesto>();
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            var command = conexion.CreateCommand();
            command.CommandText = "SELECT idPresupuesto, nombreDestinatario, FechaCreacion FROM Presupuestos;";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Presupuesto
                {
                    IdPresupuesto = reader.GetInt32(0),
                    nombreDestinatario = reader.GetString(1),
                    FechaCreacion = DateTime.Parse(reader.GetString(2))
                });
            }
            return lista;
        }

        // Obtener un Presupuesto con sus productos y cantidades.
        public Presupuesto ObtenerDetallesPorId(int id)
        {
            Presupuesto presupuesto = null;
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            // Datos del presupuesto
            var command = conexion.CreateCommand();
            command.CommandText = "SELECT IdPresupuesto, nombreDestinatario, FechaCreacion FROM Presupuestos WHERE IdPresupuesto = @id;";
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                presupuesto = new Presupuesto
                {
                    IdPresupuesto = reader.GetInt32(0),
                    nombreDestinatario = reader.GetString(1),
                    FechaCreacion = DateTime.Parse(reader.GetString(2)),
                    detalle = new List<PresupuestoDetalle>()
                };
            }

            // Detalles del presupuesto
            if (presupuesto != null)
            {
                var detalleCommand = conexion.CreateCommand();
                detalleCommand.CommandText = @"
                    SELECT pd.IdProducto, pd.Cantidad, p.descripcion, p.precio
                    FROM PresupuestosDetalle pd
                    JOIN Productos p ON pd.IdProducto = p.idProducto
                    WHERE pd.IdPresupuesto = @id;";
                detalleCommand.Parameters.AddWithValue("@id", id);

                using var detalleReader = detalleCommand.ExecuteReader();
                while (detalleReader.Read())
                {
                    var producto = new Producto
                    {
                        idProducto = detalleReader.GetInt32(0),
                        descripcion = detalleReader.GetString(2),
                        precio = detalleReader.GetInt32(3)
                    };
                    presupuesto.detalle.Add(new PresupuestoDetalle
                    {
                        producto = producto,
                        cantidad = detalleReader.GetInt32(1)
                    });
                }
            }
            return presupuesto;
        }

        // Agregar producto a un presupuesto.
        public void AgregarAPresupuesto(int idPresupuesto, int idProducto, int cantidad)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();
            string sql = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @Cantidad);";
            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            comando.Parameters.AddWithValue("@idProducto", idProducto);
            comando.Parameters.AddWithValue("@Cantidad", cantidad);
            comando.ExecuteNonQuery();
        }

        // Eliminar un Presupuesto.
        public bool Eliminar(int id)
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            // Primero eliminar los detalles
            var sqlDetalle = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @idPresupuesto;";
            using var comandoDetalle = new SqliteCommand(sqlDetalle, conexion);
            comandoDetalle.Parameters.AddWithValue("@idPresupuesto", id);
            comandoDetalle.ExecuteNonQuery();

            // Luego eliminar el presupuesto
            var sqlPresupuesto = "DELETE FROM Presupuestos WHERE IdPresupuesto = @id;";
            using var comandoPresupuesto = new SqliteCommand(sqlPresupuesto, conexion);
            comandoPresupuesto.Parameters.AddWithValue("@id", id);
            int filasAfectadas = comandoPresupuesto.ExecuteNonQuery();
            return filasAfectadas > 0; // true si eliminó algo
        }
    }
}
