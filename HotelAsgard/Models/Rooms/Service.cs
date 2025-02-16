namespace HotelAsgard.Models.Rooms;

public class Service
{
    public string Nombre { get; set; }

    public string Icono { get; set; } // Ruta de la imagen

    public bool Seleccionado { get; set; } // Para la UI
}

public static class ServiceList
{
    public static List<Service> ServiciosDisponibles = new List<Service>
    {
        new Service { Nombre = "WiFi", Icono = "/Images/Icons/wifi.png" },
        new Service { Nombre = "Aire Acondicionado", Icono = "/Images/Icons/ac.png" },
        new Service { Nombre = "Televisión", Icono = "/Images/Icons/television.png" },
        new Service { Nombre = "Bar", Icono = "/Images/Icons/bar.png" },
        new Service { Nombre = "Piscina", Icono = "/Images/Icons/swimmingpool.png" },
        new Service { Nombre = "Gimnasio", Icono = "/Images/Icons/gym.png" },
        new Service { Nombre = "Spa", Icono = "/Images/Icons/spa_black.png" },
        new Service { Nombre = "Servicio Premium", Icono = "/Images/Icons/premiumroomservice.png" },
        new Service { Nombre = "Oficina", Icono = "/Images/Icons/office.png" },
        new Service { Nombre = "Cocina", Icono = "/Images/Icons/kitchen.png" },
        new Service { Nombre = "Nevera", Icono = "/Images/Icons/fridge.png" },
        new Service { Nombre = "Cafetera", Icono = "/Images/Icons/coffemaker.png" },
        new Service { Nombre = "Billar", Icono = "/Images/Icons/billiard.png" },
        new Service { Nombre = "Balcón", Icono = "/Images/Icons/balcony.png" },
        new Service { Nombre = "Plancha", Icono = "pack://application:,,,/Images/Icons/iron.png" },
        new Service { Nombre = "Minibar", Icono = "pack://application:,,,/Images/Icons/minibar.png" }
    };
}