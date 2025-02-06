using System.Collections.ObjectModel;
using System.Windows;
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
        public RoomViewModel RoomViewModel { get; set; }


        public searchRooms()
        {
            InitializeComponent();
            RoomViewModel = new RoomViewModel();
            DataContext = RoomViewModel; // Enlaza la UI con el ViewModel
            
            
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
            BookByRoom bookByRoom = new BookByRoom();
            bookByRoom.Show();
            this.Close();
        }
        
        private void MainWindows_click(object sender, RoutedEventArgs e)
        {
            initial_view iv = new initial_view();
            iv.Show();
            this.Close();
        }
    }
    
}
