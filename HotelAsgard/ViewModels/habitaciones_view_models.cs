using System.Collections.ObjectModel;

public class HabitacionesViewModel
{
    public class Habitacion
    {
        public string Nombre { get; set; } // Nombre de la habitación
        public List<string> Imagenes { get; set; } // Lista de rutas de imágenes asociadas
        public decimal Precio { get; set; } // Precio de la habitación

        public Habitacion()
        {
            Imagenes = new List<string>(); // Inicializar la lista de imágenes para evitar null
        }
    }
    public ObservableCollection<Habitacion> Habitaciones { get; set; }

    public HabitacionesViewModel()
    {
        // Datos de ejemplo
        Habitaciones = new ObservableCollection<Habitacion>
        {
            new Habitacion
            {
                Nombre = "Habitación Odin",
                Imagenes = new List<string> { "../../Images/hab_odin.jpg" },
                Precio = 120.50m
            },
            new Habitacion
            {
                Nombre = "Habitación Pisco",
                Imagenes = new List<string> { "../../Images/hab_odin.jpg"},
                Precio = 200.00m
            },
            new Habitacion
            {
                Nombre = "Habitación Suite",
                Imagenes = new List<string> { "../../Images/hab_odin.jpg" },
                Precio = 350.75m
            },
            new Habitacion
            {
                Nombre = "Habitación Suite",
                Imagenes = new List<string> { "../../Images/hab_odin.jpg" },
                Precio = 350.75m
            },
            new Habitacion
            {
                Nombre = "Habitación Suite",
                Imagenes = new List<string> { "../../Images/hab_odin.jpg" },
                Precio = 350.75m
            },
            new Habitacion
            {
                Nombre = "Habitación Suite",
                Imagenes = new List<string> { "../../Images/hab_odin.jpg" },
                Precio = 350.75m
            },
            new Habitacion
            {
                Nombre = "Habitación Suite",
                Imagenes = new List<string> { "../../Images/hab_odin.jpg" },
                Precio = 350.75m
            },
        };
    }
}