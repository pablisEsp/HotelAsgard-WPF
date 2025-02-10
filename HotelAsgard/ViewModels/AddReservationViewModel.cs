using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using HotelAsgard.Data;
using HotelAsgard.Models;
using HotelAsgard.Models.Rooms;

namespace HotelAsgard.ViewModels
{
    public class AddReservationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Room> Habitaciones { get; set; }
        
        private BookingService _bookingService = new BookingService();
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        // Este comando se usaba en la versión con botón, puede mantenerse si lo requieres
        public ICommand SeleccionarHabitacionCommand { get; set; }

        private Room _habitacionSeleccionada;
        public Room HabitacionSeleccionada
        {
            get => _habitacionSeleccionada;
            set
            {
                if (_habitacionSeleccionada != value)
                {
                    _habitacionSeleccionada = value;
                    OnPropertyChanged();
                    if (_habitacionSeleccionada != null)
                    {
                        // Llama al método que procesa la habitación seleccionada
                        OnRoomSelected(_habitacionSeleccionada);
                    }
                }
            }
        }

        public AddReservationViewModel()
        {
            Habitaciones = new ObservableCollection<Room>();
            EditCommand = new RelayCommand(EditRoom);
            DeleteCommand = new RelayCommand(DeleteRoom);
            SeleccionarHabitacionCommand = new RelayCommand(SeleccionarHabitacion);
        }

        public async Task LoadAvailableRooms(string fechaInicio, string fechaFin, int numeroHuespedes)
        {
            Habitaciones.Clear(); 

            var habitacionesDisponibles = await _bookingService.SearchFreeRooms(fechaInicio, fechaFin, numeroHuespedes);

            if (habitacionesDisponibles != null && habitacionesDisponibles.Count > 0)
            {
                foreach (var room in habitacionesDisponibles)
                {
                    Habitaciones.Add(room);
                }
            }
            else
            {
                MessageBox.Show("No hay habitaciones disponibles para las fechas seleccionadas.", 
                                "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    
        private void EditRoom(object obj)
        {
            if (obj is Room room)
            {
                MessageBox.Show($"Editando habitación: {room.Nombre}");
            }
        }

        private void DeleteRoom(object obj)
        {
            if (obj is Room room)
            {
                Habitaciones.Remove(room);
            }
        }

        // Este método se usa si decides mantener el comando SeleccionarHabitacion
        private void SeleccionarHabitacion(object obj)
        {
            if (obj is Room room)
            {
                HabitacionSeleccionada = room;
            }
        }

        // Método que se invoca al seleccionar una habitación
        private void OnRoomSelected(Room room)
        {
            MessageBox.Show($"Habitación seleccionada: {room.Nombre}", 
                            "Selección", MessageBoxButton.OK, MessageBoxImage.Information);
            // Aquí podrías agregar lógica adicional, por ejemplo, navegar a otra vista o actualizar otras propiedades
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
