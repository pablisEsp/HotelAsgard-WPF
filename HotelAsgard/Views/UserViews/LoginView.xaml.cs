using System.Net.Http;
using System.Windows;
using HotelAsgard.Data;
using HotelAsgard.Models;

namespace HotelAsgard.Views.UserViews
{
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {

        private readonly UserApiClient _apiClient;
        private readonly BookingService _bookingService;
        public LoginView()
        {
            InitializeComponent();
            _apiClient = new UserApiClient(new HttpClient());
            _bookingService = new BookingService();
            userBox.Text = "admin@gmail.com";
            passwordBox.Password = "admin1";

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = userBox.Text.Trim();
            string password = passwordBox.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("⚠️ Debes ingresar un email y una contraseña.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool isLoggedIn = await _apiClient.LoginAsync(email, password);
            try
            {
                if (isLoggedIn)
                {
                    MessageBox.Show("✅ Inicio de sesión exitoso.", "Bienvenido", MessageBoxButton.OK, MessageBoxImage.Information);
                    initial_view initial = new initial_view();
                    Usuario? userLogged = await _bookingService.GetUser(email);
                    UsuarioSingleton.ObtenerInstancia(userLogged!);
                    initial.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("❌ Email o contraseña incorrectos.", "Error de autenticación", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
               
            }
           
        }
    }
}
