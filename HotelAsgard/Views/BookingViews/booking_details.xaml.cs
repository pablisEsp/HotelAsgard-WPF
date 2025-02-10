using System;
using System.Collections.Generic;
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
using HotelAsgard.Models;

namespace HotelAsgard.Views.BookingViews
{
    /// <summary>
    /// Lógica de interacción para booking_details.xaml
    /// </summary>
    public partial class booking_details : Window
    {
        public booking_details(Reserva reserva)
        {
            InitializeComponent();
            DataContext = reserva;
            if (reserva.Habitacion.Imagenes.Count > 0)
            {
                string urlFinal = "http://localhost:3000"+reserva.Habitacion.Imagenes[0].ToString();
                LoadImage(urlFinal);
            }
        }
        private void LoadImage(string imageUrl)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imageUrl, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // Evita problemas de caché
                bitmap.EndInit();
                HabitacionImage.Source = bitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando imagen: {ex.Message}");
            }
        }
    }
    
}
