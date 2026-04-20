using System;
using PeluqueriaITLA;

bool wantToContinue = true;
int choosenOption = 0;

Console.WriteLine("=== PELUQUERIA ITLA ===\n");

while (wantToContinue)
{
    Console.WriteLine("\n=== MENU PRINCIPAL ===");
    Console.WriteLine("1. Registrar Cliente");
    Console.WriteLine("2. Ver Clientes");
    Console.WriteLine("3. Agendar Cita");
    Console.WriteLine("4. Ver Citas");
    Console.WriteLine("5. Cancelar Cita");
    Console.WriteLine("6. Marcar Cita como Hecha");
    Console.WriteLine("7. Reporte de Ganancias");
    Console.WriteLine("\n--- GESTION DE SERVICIOS ---");
    Console.WriteLine("8. Ver Servicios");
    Console.WriteLine("9. Agregar Servicio");
    Console.WriteLine("10. Modificar Servicio");
    Console.WriteLine("11. Eliminar Servicio");
    Console.WriteLine("\n12. Salir");
    Console.Write("Opcion: ");

    choosenOption = Convert.ToInt32(Console.ReadLine());

    switch (choosenOption)
    {
        case 1:
            PeluqueriaHelper.RegistrarCliente();
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
            PeluqueriaHelper.CancelarCita();
            break;
        case 6:
            PeluqueriaHelper.MarcarCitaHecha();
            break;
        case 7:
            PeluqueriaHelper.ReporteGanancias();
            break;
        case 8:
            PeluqueriaHelper.VerServicios();
            break;
        case 9:
            PeluqueriaHelper.AgregarServicio();
            break;
        case 10:
            PeluqueriaHelper.ModificarServicio();
            break;
        case 11:
            PeluqueriaHelper.EliminarServicio();
            break;
        case 12:
            wantToContinue = false;
            break;
        default:
            Console.WriteLine("Opcion no valida");
            break;
    }
}

Console.WriteLine("\nCerrando la aplicacion...");
Console.ReadKey();