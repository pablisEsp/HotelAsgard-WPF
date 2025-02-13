using System.Collections.ObjectModel;
using System.Windows;
using HotelAsgard.Data;
using HotelAsgard.Models;
using HotelAsgard.Models.Rooms;

namespace HotelAsgard.Views.BookingViews
{
    public partial class BookByRoom : Window
    {
        public Room SelectedRoom { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int NumeroHuespedes { get; set; }
        
        public ObservableCollection<Usuario> Usuarios { get; set; } = new();
        public Usuario UsuarioSeleccionado { get; set; }

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
    }
}
