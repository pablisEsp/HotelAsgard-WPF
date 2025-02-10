using System.Collections.ObjectModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HotelAsgard.Data;
using HotelAsgard.Models;
using HotelAsgard.ViewModels;
using HotelAsgard.Views.UserViews;

namespace HotelAsgard.Views.BookingViews
{
    public partial class BookingListView : Window
    {
        private readonly BookingService _bookingService;
        private BookingListViewModel _viewModel;

        public BookingListView()
        {
            InitializeComponent();

            
            _bookingService = new BookingService();
            _viewModel = new BookingListViewModel();
            DataContext = _viewModel;

            
            LoadBookingsAsync();
        }

        private async void LoadBookingsAsync()
        {
            try
            {
                // Mostrar indicador de carga (opcional)
                MessageBox.Show("Cargando reservas...");

                // Obtener las reservas desde la API
                var bookings = await _bookingService.GetBookings();

                if (bookings != null && bookings.Count > 0)
                {
                    // Actualizar el modelo con los datos obtenidos
                    foreach (var booking in bookings)
                    {
                        _viewModel.Reservas.Add(booking);
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron reservas.");
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error de red: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}");
            }
        }

        // Resto de métodos...
        private void DatePicker_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true; // Cancela la entrada de texto
        }

        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter && e.Key != Key.Tab && e.Key != Key.Escape)
            {
                e.Handled = true; // Cancela la entrada de teclado
            }
        }

        private void DatePicker_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender == FechaInicio)
            {
                FechaInicio.IsDropDownOpen = true;
            }
            else
            {
                FechaFin.IsDropDownOpen = true;
            }
        }

        private void MainWindows_click(object sender, RoutedEventArgs e)
        {
            initial_view iv = new initial_view();
            iv.Show();
            this.Close();
        }

        private void Perfil_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUser = new AddUserWindow();
            addUser.Show();
            this.Close();
        }

        private void BuscarUsuario_Click(object sender, RoutedEventArgs e)
        {
            SearchUserWindow searchUser = new SearchUserWindow();
            searchUser.Show();
            this.Close();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUser = new AddUserWindow();
            addUser.Show();
            this.Close();
        }

        private void SearchBooking_Click(object sender, RoutedEventArgs e)
        {
            AddReservation addReservation = new AddReservation();
            addReservation.Show();
            this.Close();
        }

        private void AddBooking_Click(object sender, RoutedEventArgs e)
        {
            BookByRoom bookByRoom = new BookByRoom();
            bookByRoom.Show();
            this.Close();
        }
        
 
        private async void SearchBooking_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string? fechaInicio = null;
                string? fechaFin = null;
                string? codigo = null;
                string? nombreUsuario = null;
                string? tipoUsuario = null;

                // Obtener valores de los controles de la interfaz
                if (FechaInicio.SelectedDate.HasValue)
                {
                    fechaInicio = FechaInicio.SelectedDate.Value.ToString("yyyy-MM-dd"); // Format date as YYYY-MM-DD for API
                }
                if (FechaFin.SelectedDate.HasValue)
                {
                    fechaFin = FechaFin.SelectedDate.Value.ToString("yyyy-MM-dd"); // Format date as YYYY-MM-dd for API
                }
                codigo = CodigoTextBox.Text;
                nombreUsuario = UsuarioTextBox.Text;
                var selectedItem = TipoComboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    tipoUsuario = selectedItem.Content.ToString();
                }
                try
                {
             
                    var bookings = await _bookingService.SearchBookings(fechaInicio: fechaInicio, fechaFin: fechaFin, codigo: codigo, nombre: nombreUsuario, tipo: tipoUsuario);

                    if (bookings != null)
                    {
                    
                        _viewModel.Reservas.Clear();
                        if (bookings.Count > 0)
                        {
                            // Actualizar el modelo con los datos obtenidos
                            foreach (var booking in bookings)
                            {
                                _viewModel.Reservas.Add(booking);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron reservas con los criterios de búsqueda.", "Sin Resultados", MessageBoxButton.OKCancel);
                        }
                    }
                
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show("No se encontraron reservas con los criterios de búsqueda.", "Sin Resultados", MessageBoxButton.OK);
                    _viewModel.Reservas.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error inesperado al buscar reservas: {ex.Message}", "Error", MessageBoxButton.OK);
                    _viewModel.Reservas.Clear();
                }
            }
            catch (Exception y)
            {
                MessageBox.Show("No se encontraron reservas con los criterios de búsqueda.", "Sin Resultados", MessageBoxButton.OK);
                _viewModel.Reservas.Clear();
            }
        }

        private void ClearFechaFin_Click(object sender, RoutedEventArgs e)
        {
            FechaFin.ClearValue(DatePicker.SelectedDateProperty);
        }

        private void ClearFechaInicio_Click(object sender, RoutedEventArgs e)
        {
            FechaInicio.ClearValue(DatePicker.SelectedDateProperty);

        }

        private void OpenDetails_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedReserva = ReservasDataGrid.SelectedItem as Reserva;
            if (selectedReserva != null)
            {
                booking_details bd = new booking_details(selectedReserva);
                bd.ShowDialog();
            }
        }
    }
}