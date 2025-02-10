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
using HotelAsgard.Models.Rooms;

namespace HotelAsgard.Views.BookingViews
{
    /// <summary>
    /// Lógica de interacción para BookByRoom.xaml
    /// </summary>
    public partial class BookByRoom : Window
    {
        public Room SelectedRoom { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int NumeroHuespedes { get; set; }

        public BookByRoom(Room selectedRoom, DateTime fechaEntrada, DateTime fechaSalida, int numeroHuespedes)
        {
            InitializeComponent();
            SelectedRoom = selectedRoom;
            FechaEntrada = fechaEntrada;
            FechaSalida = fechaSalida;
            NumeroHuespedes = numeroHuespedes;
            DataContext = this;
        }

      
    }
    


}
