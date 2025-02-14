using System.Net.Http;
using System.Windows;
using System.Windows.Controls;


namespace HotelAsgard.Views.UserViews
{
    /// <summary>
    /// Lógica de interacción para AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private readonly UserApiClient _apiClient;
        private string image = "";
        public AddUserWindow()
        {
            InitializeComponent();
            _apiClient = new UserApiClient(new HttpClient());

        }

        public async void addUser(object sender, RoutedEventArgs e)
        {
            RegistroNuevoUser();
        }
        
        private async Task RegistroNuevoUser()
        {

            try
            {
                // Validar que los campos obligatorios no estén vacíos
                if (string.IsNullOrWhiteSpace(textnombre.Text) ||
                    string.IsNullOrWhiteSpace(textapellido1.Text) ||
                    string.IsNullOrWhiteSpace(textemail.Text) ||
                    string.IsNullOrWhiteSpace(textTelefono.Text) ||
                    string.IsNullOrWhiteSpace(textdni.Text) ||
                    string.IsNullOrWhiteSpace(((ComboBoxItem)tipoUsers.SelectedItem)?.Content?.ToString()) ||
                    string.IsNullOrWhiteSpace(textPassword.Text))
                {
                    MessageBox.Show("Todos los campos obligatorios deben ser completados.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Capturar datos desde la interfaz
                var user = new UserModel
                {
                    Nombre = textnombre.Text,
                    Email = textemail.Text,
                    Password = textPassword.Text,
                    Apellido1 = textapellido1.Text,
                    Apellido2 = textapellido2.Text, // Puede ser null
                    DniPasaporte = textdni.Text,
                    Movil = textTelefono.Text,
                    FechaNacimiento = fechaNacimiento.SelectedDate ?? DateTime.MinValue,
                    Tipo = ((ComboBoxItem)tipoUsers.SelectedItem).Content.ToString(),
                    Avatar = image
                    
                };

                // Llamar a la API para registrar el usuario
                bool result = await _apiClient.RegisterUserAsync(user);

                if (result)
                {
                    MessageBox.Show("Usuario registrado con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    initial_view initialView = new initial_view();
                    initialView.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Error al registrar el usuario. Inténtelo nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Excepción", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
