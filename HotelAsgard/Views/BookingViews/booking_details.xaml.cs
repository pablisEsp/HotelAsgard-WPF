using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using HotelAsgard.Data;
using HotelAsgard.Models;


namespace HotelAsgard.Views.BookingViews
{
    /// <summary>
    /// Lógica de interacción para booking_details.xaml
    /// </summary>
    public partial class booking_details : Window
    {   
        private readonly BookingListView _parentWindow; 
        bool IsAdmin = false;
        BookingService _booBookingService ;
        public booking_details(Reserva reserva, BookingListView parentWindow)
        {
            //aqui esta el Singleton
            InitializeComponent();
            _booBookingService = new BookingService();
            _parentWindow = parentWindow; 
            Broder.SetValue(Border.VisibilityProperty,
            UsuarioSingleton.UsuarioActual.Tipo == "admin" ? Visibility.Visible : Visibility.Collapsed);
            DataContext = reserva;
            if (reserva.Habitacion.Imagenes.Count > 0)
            {
                string urlFinal = "http://localhost:3000"+reserva.Habitacion.Imagenes[0];
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

        private async void DeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            if (((Reserva)DataContext).FechaInicio < DateTime.Now)
            {
                MessageBox.Show("No se puede eliminar una reserva que ya se ha iniciado");
                return; 
                    
            }
            MessageBoxResult result = MessageBox.Show(
                "¿Estás seguro de que deseas eliminar la reserva?", 
                "Confirmación", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                bool deleted = await _booBookingService.deleteBooking(((Reserva)DataContext).Codigo!);
                if (deleted)
                {
                    ShowTemporaryMessage("Reserva eliminada correctamente", 500);
                }
            }
        }
        private async void ShowTemporaryMessage(string message, int duration)
        {
            MessageBox.Show(message, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            await Task.Delay(duration);
            _parentWindow._viewModel.Reservas.Clear();
            _parentWindow.LoadBookingsAsync();
            this.Close();
        }
    }
    
}
