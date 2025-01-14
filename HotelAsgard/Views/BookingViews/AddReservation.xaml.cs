using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelAsgard.Views.BookingViews
{
    /// <summary>
    /// Lógica de interacción para AddReservation.xaml
    /// </summary>
    public partial class AddReservation : Window
    {
        public AddReservation()
        {
            InitializeComponent();

            Habitaciones = new ObservableCollection<Habitacion>
            {
                new Habitacion { Imagen = "Images/hab_odin.jpg", Nombre = "Habitación Odin", Precio = "36€ / noche" },
                new Habitacion { Imagen = "Images/hab_valhalla.jpg", Nombre = "Habitación Valhalla", Precio = "42€ / noche" },
                new Habitacion { Imagen = "Images/hab_freyja.jpg", Nombre = "Habitación Freyja", Precio = "50€ / noche" }
            };

            DataContext = this;
        }

        public ObservableCollection<Habitacion> Habitaciones { get; }

        public class Habitacion
        {
            public string Imagen { get; set; }
            public string Nombre { get; set; }
            public string Precio { get; set; }
        }
    }
}
