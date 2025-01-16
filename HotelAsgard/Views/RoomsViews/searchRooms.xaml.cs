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

namespace HotelAsgard.Views.RoomsViews
{
    /// <summary>
    /// Lógica de interacción para searchRooms.xaml
    /// </summary>

    public partial class searchRooms : Window
    {
        public ObservableCollection<Habitacion> Habitaciones { get; set; }

        public searchRooms()
        {
            InitializeComponent();
            addRoom a = new addRoom();
            a.Show();
            Habitaciones = new ObservableCollection<Habitacion>
            {
                new Habitacion
                {
                    Codigo = 1,
                    Nombre = "Suite Deluxe",
                    NumHuespedes = 2,
                    Descripcion = "Una suite con vistas al mar.",
                    Precio = 200,
                    Oferta = true,
                    Cuna = false,
                    CamaExtra = false,
                    Ocupada = false,
                    Fecha = "16/01/2025"
                },
                new Habitacion
                {
                    Codigo = 2,
                    Nombre = "Habitación Familiar",
                    NumHuespedes = 4,
                    Descripcion = "Perfecta para familias.",
                    Precio = 150,
                    Oferta = false,
                    Cuna = true,
                    CamaExtra = true,
                    Ocupada = true,
                    Fecha = "16/01/2025"
                }
            };
            DataContext = this;
        }
    }

    public class Habitacion
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public int NumHuespedes { get; set; }
        public string Descripcion { get; set; }
        public int Precio { get; set; }
        public bool Oferta { get; set; }
        public bool Cuna { get; set; }
        public bool CamaExtra { get; set; }
        public bool Ocupada { get; set; }
        public string Fecha { get; set; }

    }
}
