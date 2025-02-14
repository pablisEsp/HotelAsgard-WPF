using System.Net.Http;
using System.Windows;
using HotelAsgard.ViewModels;

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
