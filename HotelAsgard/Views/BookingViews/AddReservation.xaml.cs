using System.Globalization;
using HotelAsgard.Views.UserViews;
using System.Windows;
using System.Windows.Input;
using HotelAsgard.ViewModels;

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
              fechaInicio.SelectedDate = DateTime.Today;
              
              fechaFin.SelectedDate = DateTime.Today.AddDays(2);

              DataContext = new AddReservationViewModel();
        }
        private void DatePicker_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true; 
        }

        
        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Permite solo teclas de navegación (flechas, tab, etc.)
            if (e.Key != Key.Enter && e.Key != Key.Tab && e.Key != Key.Escape)
            {
                e.Handled = true; // Cancela la entrada de teclado
            }
        }

        
        private void DatePicker_GotFocus(object sender, RoutedEventArgs e)
        {
            if(sender == fechaInicio)
            {
                fechaInicio.IsDropDownOpen = true;

            }
            else
            {
                fechaFin.IsDropDownOpen = true;
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
            // Lógica para la opción "Perfil"
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

        private async void SearchFreeRooms_Click(object sender, RoutedEventArgs e)
        {
            if (fechaInicio.SelectedDate == null || fechaFin.SelectedDate == null)
            {
                MessageBox.Show("Por favor, seleccione ambas fechas.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (fechaInicio.SelectedDate >= fechaFin.SelectedDate)
            {
                MessageBox.Show("La fecha de entrada debe ser anterior a la fecha de salida.", "Fechas inválidas", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Convertir fechas a formato dd/MM/yyyy
            string fechaInicioStr = fechaInicio.SelectedDate.Value.ToString("dd/MM/yyyy");
            string fechaFinStr = fechaFin.SelectedDate.Value.ToString("dd/MM/yyyy");

            // Obtener el número de huéspedes directamente del ComboBox
            int numeroHuespedes = int.Parse(NumHuespedes.Text);

            // Llamar al ViewModel para buscar habitaciones
            if (DataContext is AddReservationViewModel viewModel)
            {
                await viewModel.LoadAvailableRooms(fechaInicioStr, fechaFinStr, numeroHuespedes);
            }
        }

    }
}

