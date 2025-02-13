using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using HotelAsgard.Data;
using HotelAsgard.Models.Rooms;
using HotelAsgard.ViewModels;
using HotelAsgard.Views.BookingViews;
using HotelAsgard.Views.UserViews;

namespace HotelAsgard.Views.RoomsViews
{
    /// <summary>
    /// Lógica de interacción para searchRooms.xaml
    /// </summary>

    public partial class searchRooms : Window
    {
        //public ObservableCollection<Habitacion> Habitaciones { get; set; }
        public RoomVM RoomVm { get; set; }


        public searchRooms()
        {
            InitializeComponent();
            RoomVm = new RoomVM();
            DataContext = RoomVm; // Enlaza la UI con el ViewModel
            
            
        }

        private void editRoom_Click(object sender, RoutedEventArgs e)
        {
            //Habitaciones[0]
            addRoom window = new addRoom("Editar habitación", "Editar");
            window.Show(); 
        }

        private void deleteRoom_Click(object sender, RoutedEventArgs e)
        {
            // delete room
        }

        private void roomInfo_Click(object sender, RoutedEventArgs e)
        {

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
           AddReservation addReservation = new AddReservation();
           addReservation.Show();
            this.Close();
        }
        
        private void MainWindows_click(object sender, RoutedEventArgs e)
        {
            initial_view iv = new initial_view();
            iv.Show();
            this.Close();
        }
    
        private RoomService _roomService = new RoomService();

        private async void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Room room)
            {
                try
                {
                    // Invertir el estado de Habilitada basado en la casilla marcada
                    bool newStatus = checkBox.IsChecked == true;
                    room.Habilitada = newStatus;

                    // Llamar a la API para actualizar el estado en la base de datos
                    bool success = await _roomService.ToggleRoomAvailability(room);
            
                    if (!success)
                    {
                        // Si la API falla, revertimos el cambio en la UI
                        room.Habilitada = !newStatus;
                        MessageBox.Show("Error al actualizar la habitación.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                    MessageBox.Show("Habitación " + room.Nombre + (room.Habilitada ? " Habilitada" : " Deshabilitada"));
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        
    }
    
}