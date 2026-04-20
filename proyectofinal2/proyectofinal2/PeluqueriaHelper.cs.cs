using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PeluqueriaITLA
{
    public static class PeluqueriaHelper
    {
        private static string connectionString = "Server=.\\SQLEXPRESS;Database=PELUQUERIA_ITLA;Integrated Security=True;";

        public static void AgregarCliente()
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

            Console.WriteLine("\n✅ Cliente registrado exitosamente");
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
                    int id = (int)reader["ID"];
                    string nombre = reader["Nombre"].ToString();
                    Console.WriteLine($"{id}. {nombre}");
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
                    int id = (int)reader["ID"];
                    string nombre = reader["Nombre"].ToString();
                    decimal precio = (decimal)reader["Precio"];
                    Console.WriteLine($"{id}. {nombre} - RD${precio}");
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
                SqlCommand cmd = new SqlCommand("INSERT INTO Citas (ClienteID, ServicioID, Fecha) VALUES (@c, @s, @f)", conn);
                cmd.Parameters.AddWithValue("@c", clienteId);
                cmd.Parameters.AddWithValue("@s", servicioId);
                cmd.Parameters.AddWithValue("@f", fecha);
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            Console.WriteLine($"\n✅ Cita agendada exitosamente");
        }

        public static void VerCitas()
        {
            Console.Clear();
            Console.WriteLine("--- LISTA DE CITAS ---\n");
            Console.WriteLine("ID     Cliente              Servicio             Precio     Fecha");
            Console.WriteLine("---    -----------------    -----------------    ------     ----------");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT c.ID, c.Fecha, cl.Nombre as ClienteNombre, s.Nombre as ServicioNombre, s.Precio
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
                    Console.WriteLine($"{id,-6} {cliente,-20} {servicio,-20} RD${precio,-7} {fecha:dd/MM/yyyy}");
                }
                conn.Close();
            }
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
                                                  JOIN Servicios s ON c.ServicioID = s.ID", conn);
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
    }
}