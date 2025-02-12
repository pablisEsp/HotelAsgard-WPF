using HotelAsgard.Views.UserViews;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
           

            DataContext = new AddReservationViewModel();
        }
        private void DatePicker_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true; // Cancela la entrada de texto
        }

        // Evita que el usuario use el teclado para editar el texto
        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Permite solo teclas de navegación (flechas, tab, etc.)
            if (e.Key != Key.Enter && e.Key != Key.Tab && e.Key != Key.Escape)
            {
                e.Handled = true; // Cancela la entrada de teclado
            }
        }

        // Abre el calendario cuando el DatePicker recibe el foco
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
    }
}

