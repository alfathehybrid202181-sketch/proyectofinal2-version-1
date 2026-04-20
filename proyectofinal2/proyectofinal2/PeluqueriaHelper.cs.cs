using System;
using System.Data.SqlClient;

namespace PeluqueriaITLA
{
    public static class PeluqueriaHelper
    {
        private static string connectionString = "Server=.\\SQLEXPRESS;Database=PELUQUERIA_ITLA;Integrated Security=True;";

        public static void RegistrarCliente()
        {
            Console.Clear();
            Console.WriteLine("--- REGISTRAR CLIENTE ---\n");

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();
            Console.Write("Telefono: ");
            string telefono = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Clientes (Nombre, Telefono) VALUES (@n, @t)", conn);
                cmd.Parameters.AddWithValue("@n", nombre);
                cmd.Parameters.AddWithValue("@t", telefono);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            Console.WriteLine("\nCliente registrado exitosamente");
        }

        public static void VerClientes()
        {
            Console.Clear();
            Console.WriteLine("--- LISTA DE CLIENTES ---\n");
            Console.WriteLine("ID     Nombre               Telefono");
            Console.WriteLine("---    -----------------    ----------");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT ID, Nombre, Telefono FROM Clientes", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["ID"];
                    string nombre = reader["Nombre"].ToString();
                    string telefono = reader["Telefono"].ToString();
                    Console.WriteLine($"{id,-6} {nombre,-20} {telefono}");
                }
                conn.Close();
            }
        }

        public static void AgendarCita()
        {
            Console.Clear();
            Console.WriteLine("--- AGENDAR CITA ---\n");

            Console.WriteLine("CLIENTES DISPONIBLES:");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT ID, Nombre FROM Clientes", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["ID"]}. {reader["Nombre"]}");
                }
                conn.Close();
            }

            Console.Write("\nID del cliente: ");
            int clienteId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\nSERVICIOS DISPONIBLES:");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT ID, Nombre, Precio FROM Servicios", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["ID"]}. {reader["Nombre"]} - RD${reader["Precio"]}");
                }
                conn.Close();
            }

            Console.Write("\nID del servicio: ");
            int servicioId = Convert.ToInt32(Console.ReadLine());

            Console.Write("\nFecha (yyyy-mm-dd): ");
            DateTime fecha = DateTime.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Citas (ClienteID, ServicioID, Fecha, Estado) VALUES (@c, @s, @f, 'Pendiente')", conn);
                cmd.Parameters.AddWithValue("@c", clienteId);
                cmd.Parameters.AddWithValue("@s", servicioId);
                cmd.Parameters.AddWithValue("@f", fecha);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            Console.WriteLine($"\nCita agendada exitosamente");
        }

        public static void VerCitas()
        {
            Console.Clear();
            Console.WriteLine("--- LISTA DE CITAS ---\n");
            Console.WriteLine("ID     Cliente              Servicio             Precio     Fecha       Estado");
            Console.WriteLine("---    -----------------    -----------------    ------     ----------  ----------");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT c.ID, c.Fecha, c.Estado, cl.Nombre as ClienteNombre, s.Nombre as ServicioNombre, s.Precio
                                                  FROM Citas c
                                                  JOIN Clientes cl ON c.ClienteID = cl.ID
                                                  JOIN Servicios s ON c.ServicioID = s.ID
                                                  ORDER BY c.Fecha DESC", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["ID"];
                    string cliente = reader["ClienteNombre"].ToString();
                    string servicio = reader["ServicioNombre"].ToString();
                    decimal precio = (decimal)reader["Precio"];
                    DateTime fecha = (DateTime)reader["Fecha"];
                    string estado = reader["Estado"].ToString();
                    Console.WriteLine($"{id,-6} {cliente,-20} {servicio,-20} RD${precio,-7} {fecha:dd/MM/yyyy}  {estado}");
                }
                conn.Close();
            }
        }

        public static void CancelarCita()
        {
            Console.Clear();
            Console.WriteLine("--- CANCELAR CITA ---\n");

            VerCitas();
            Console.Write("\nID de la cita a cancelar: ");
            int citaId = Convert.ToInt32(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Citas SET Estado = 'Cancelada' WHERE ID = @id", conn);
                cmd.Parameters.AddWithValue("@id", citaId);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            Console.WriteLine("\nCita cancelada exitosamente");
        }

        public static void MarcarCitaHecha()
        {
            Console.Clear();
            Console.WriteLine("--- MARCAR CITA COMO HECHA ---\n");

            VerCitas();
            Console.Write("\nID de la cita a marcar como hecha: ");
            int citaId = Convert.ToInt32(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Citas SET Estado = 'Hecha' WHERE ID = @id", conn);
                cmd.Parameters.AddWithValue("@id", citaId);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            Console.WriteLine("\nCita marcada como hecha exitosamente");
        }

        public static void ReporteGanancias()
        {
            Console.Clear();
            Console.WriteLine("--- REPORTE DE GANANCIAS ---\n");

            decimal totalGeneral = 0;
            decimal totalHoy = 0;
            DateTime hoy = DateTime.Now.Date;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT c.Fecha, s.Precio
                                                  FROM Citas c
                                                  JOIN Servicios s ON c.ServicioID = s.ID
                                                  WHERE c.Estado = 'Hecha'", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime fecha = (DateTime)reader["Fecha"];
                    decimal precio = (decimal)reader["Precio"];
                    totalGeneral += precio;
                    if (fecha.Date == hoy)
                    {
                        totalHoy += precio;
                    }
                }
                conn.Close();
            }

            Console.WriteLine($"Fecha actual: {hoy:dd/MM/yyyy}");
            Console.WriteLine($"Total ganado hoy: RD${totalHoy}");
            Console.WriteLine($"Total ganado en general: RD${totalGeneral}");
        }

        public static void VerServicios()
        {
            Console.Clear();
            Console.WriteLine("--- LISTA DE SERVICIOS ---\n");
            Console.WriteLine("ID     Nombre                Precio");
            Console.WriteLine("---    -----------------    ----------");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT ID, Nombre, Precio FROM Servicios", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["ID"];
                    string nombre = reader["Nombre"].ToString();
                    decimal precio = (decimal)reader["Precio"];
                    Console.WriteLine($"{id,-6} {nombre,-20} RD${precio}");
                }
                conn.Close();
            }
        }

        public static void AgregarServicio()
        {
            Console.Clear();
            Console.WriteLine("--- AGREGAR SERVICIO ---\n");

            Console.Write("Nombre del servicio: ");
            string nombre = Console.ReadLine();
            Console.Write("Precio: RD$ ");
            decimal precio = Convert.ToDecimal(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Servicios (Nombre, Precio) VALUES (@n, @p)", conn);
                cmd.Parameters.AddWithValue("@n", nombre);
                cmd.Parameters.AddWithValue("@p", precio);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            Console.WriteLine("\nServicio agregado exitosamente");
        }

        public static void ModificarServicio()
        {
            Console.Clear();
            Console.WriteLine("--- MODIFICAR SERVICIO ---\n");

            VerServicios();
            Console.Write("\nID del servicio a modificar: ");
            int servicioId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Nuevo nombre (dejar vacio para no cambiar): ");
            string nuevoNombre = Console.ReadLine();

            Console.Write("Nuevo precio (dejar 0 para no cambiar): ");
            decimal nuevoPrecio = Convert.ToDecimal(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (!string.IsNullOrEmpty(nuevoNombre))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Servicios SET Nombre = @n WHERE ID = @id", conn);
                    cmd.Parameters.AddWithValue("@n", nuevoNombre);
                    cmd.Parameters.AddWithValue("@id", servicioId);
                    cmd.ExecuteNonQuery();
                }

                if (nuevoPrecio > 0)
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Servicios SET Precio = @p WHERE ID = @id", conn);
                    cmd.Parameters.AddWithValue("@p", nuevoPrecio);
                    cmd.Parameters.AddWithValue("@id", servicioId);
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
            Console.WriteLine("\nServicio modificado exitosamente");
        }

        public static void EliminarServicio()
        {
            Console.Clear();
            Console.WriteLine("--- ELIMINAR SERVICIO ---\n");

            VerServicios();
            Console.Write("\nID del servicio a eliminar: ");
            int servicioId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Esta seguro? (s/n): ");
            string confirmacion = Console.ReadLine();

            if (confirmacion.ToLower() == "s")
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Servicios WHERE ID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", servicioId);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                Console.WriteLine("\nServicio eliminado exitosamente");
            }
            else
            {
                Console.WriteLine("\nOperacion cancelada");
            }
        }
    }
}