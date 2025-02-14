using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace HotelAsgard.Views.UserViews
{
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {

        private readonly UserApiClient _apiClient;

        public LoginView()
        {
            InitializeComponent();
            _apiClient = new UserApiClient(new HttpClient());
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

            if (isLoggedIn)
            {
                MessageBox.Show("✅ Inicio de sesión exitoso.", "Bienvenido", MessageBoxButton.OK, MessageBoxImage.Information);

                initial_view initial = new initial_view();
                initial.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("❌ Email o contraseña incorrectos.", "Error de autenticación", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
