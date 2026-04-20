using System;
using System.Collections.Generic;
using PeluqueriaITLA;

List<Cliente> clientes = new List<Cliente>();
List<Cita> citas = new List<Cita>();

bool wantToContinue = true;
int choosenOption = 0;

Console.WriteLine("=== PELUQUERIA ITLA ===\n");

while (wantToContinue)
{
    Console.WriteLine("\n1. Registrar Cliente");
    Console.WriteLine("2. Ver Clientes");
    Console.WriteLine("3. Agendar Cita");
    Console.WriteLine("4. Ver Citas");
    Console.WriteLine("5. Reporte de Ganancias");
    Console.WriteLine("6. Salir");
    Console.Write("Opcion: ");

    choosenOption = Convert.ToInt32(Console.ReadLine());

    switch (choosenOption)
    {
        case 1:
            PeluqueriaHelper.AgregarCliente();
            break;
        case 2:
            PeluqueriaHelper.VerClientes();
            break;
        case 3:
            PeluqueriaHelper.AgendarCita();
            break;
        case 4:
            PeluqueriaHelper.VerCitas();
            break;
        case 5:
            PeluqueriaHelper.ReporteGanancias();
            break;
        case 6:
            wantToContinue = false;
            break;
        default:
            Console.WriteLine("Opcion no valida");
            break;
    }
}

Console.WriteLine("\nCerrando la aplicacion...");
Console.ReadKey();