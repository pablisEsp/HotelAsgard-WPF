using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using HotelAsgard.Data;
using HotelAsgard.Models;
using HotelAsgard.Models.Rooms;

namespace HotelAsgard.Views.BookingViews
{
    public partial class BookByRoom : Window
    {
        private decimal _precioFinal;
        public Room SelectedRoom { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int NumeroHuespedes { get; set; }

        public ObservableCollection<Usuario> Usuarios { get; set; } = new();
        private Usuario _usuarioSeleccionado;

        
        public string usuarioInfo = "no hay ningun usuario seleccionado";
        

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public Usuario UsuarioSeleccionado
        {
            get => _usuarioSeleccionado;
            set
            {
                _usuarioSeleccionado = value;
                UserInfo.Text =
                    $"{_usuarioSeleccionado.Nombre} {_usuarioSeleccionado.Apellido1} -- {_usuarioSeleccionado.Email}";
            }
        }

        private readonly BookingService _apiService;

        public BookByRoom(Room selectedRoom, DateTime fechaEntrada, DateTime fechaSalida, int numeroHuespedes)
        {
            InitializeComponent();
            SelectedRoom = selectedRoom;
            FechaEntrada = fechaEntrada;
            FechaSalida = fechaSalida;
            NumeroHuespedes = numeroHuespedes;
            _apiService = new BookingService();

            DataContext = this;
            CalcularPrecioFinal();
            
            _ = CargarUsuariosAsync();
        }

        private async Task CargarUsuariosAsync()
        {
            var usuarios = await _apiService.GetUsers()!;
            Usuarios.Clear();
            foreach (var usuario in usuarios)
            {
                Usuarios.Add(usuario);
            }
            
        }
        private void CalcularPrecioFinal()
        {
            if (SelectedRoom != null && FechaSalida > FechaEntrada)
            {
                int noches = (FechaSalida - FechaEntrada).Days;
                _precioFinal = noches * SelectedRoom.Precio;
                Precio.Text = _precioFinal.ToString(CultureInfo.InvariantCulture)+"€";
            }
            else
            {
                _precioFinal = 0;
                Precio.Text = _precioFinal.ToString(CultureInfo.InvariantCulture);

            }
        }
        private async Task CrearReserva()
        {
            if (SelectedRoom == null || UsuarioSeleccionado == null)
            {
                MessageBox.Show("Selecciona una habitación y un usuario antes de continuar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Obtener el próximo código de reserva desde el API
            string? nuevoCodigo = await _apiService.GetNextBookingCodeAsync();
            if (string.IsNullOrEmpty(nuevoCodigo))
            {
                MessageBox.Show("No se pudo generar el código de reserva.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var nuevaReserva = new CreaReserva
            {
                codigo = nuevoCodigo, // Asigna el código obtenido de la API
                fechaInicio = FechaEntrada.ToShortDateString(),
                fechaFin = FechaSalida.ToShortDateString(),
                habitacion = SelectedRoom._id, // MongoDB ObjectId de la habitación
                usuario = UsuarioSeleccionado._id, // MongoDB ObjectId del usuario
                precio = (double)_precioFinal // Convertimos a double por si la API lo requiere
            };

            bool success = await _apiService.CreateBookingAsync(nuevaReserva);

            if (success)
            {
                MessageBox.Show($"Reserva creada con éxito! Código: {nuevoCodigo}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Hubo un error al crear la reserva.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _ = CrearReserva();
            await  Task.Delay(2000); 
            this.Close();
        }
    }
    
    

}

public class CreaReserva
{
    public string codigo { get; set; } = null!;
    public string fechaInicio { get; set; }
    public string fechaFin { get; set; }
    public string habitacion { get; set; } = null!; // ID de la habitación (MongoDB ObjectId)
    public string usuario { get; set; } = null!; // ID del usuario (MongoDB ObjectId)
    public double precio { get; set; }
}


