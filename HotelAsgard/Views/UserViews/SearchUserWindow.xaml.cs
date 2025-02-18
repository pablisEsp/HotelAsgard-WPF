using System.Net.Http;
using System.Windows;
using HotelAsgard.ViewModels;
using HotelAsgard.Views.BookingViews;

namespace HotelAsgard.Views.UserViews
{
    /// <summary>
    /// Lógica de interacción para SearchUserWindow.xaml
    /// </summary>
    public partial class SearchUserWindow : Window
    {
        private readonly UserApiClient _userApiClient;
        private userViewModel _viewModel;

        public SearchUserWindow()
        {
            InitializeComponent();
            _userApiClient = new UserApiClient(new HttpClient());
            _viewModel = new userViewModel();
            DataContext = _viewModel;
            
            LoadUsersDataGrid();
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
            BookingListView bookingListView = new BookingListView();   
            bookingListView.Show();
            this.Close();
        }

        private void AddBooking_Click(object sender, RoutedEventArgs e)
        {
            AddReservation addReservation = new AddReservation();
            addReservation.Show();
            addReservation.Show();
            this.Close();
        }

        private async void LoadUsersDataGrid()
        {
            try
            {

                // Obtener los usuarios desde la API
                var users = await _userApiClient.GetAllUsersAsync();

                if (users != null && users.Count > 0)
                {
                    
                    // Actualizar el modelo con los datos obtenidos
                    foreach (var user in users)
                    {
                        _viewModel.UserModels.Add(user);
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
    }
}
