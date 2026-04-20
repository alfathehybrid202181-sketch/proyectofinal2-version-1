using System;

namespace PeluqueriaITLA
{
    public class Cita
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public int ServicioId { get; set; }
        public string ServicioNombre { get; set; }
        public decimal Precio { get; set; }
        public DateTime Fecha { get; set; }

        public Cita()
        {
        }

        public Cita(int id, int clienteId, string clienteNombre, int servicioId, string servicioNombre, decimal precio, DateTime fecha)
        {
            Id = id;
            ClienteId = clienteId;
            ClienteNombre = clienteNombre;
            ServicioId = servicioId;
            ServicioNombre = servicioNombre;
            Precio = precio;
            Fecha = fecha;
        }
    }
}