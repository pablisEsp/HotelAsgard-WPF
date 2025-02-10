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
    public class AddReservationViewModel 
    {
        public ObservableCollection<Room> Habitaciones { get; set; }
        
        private BookingService _bookingService = new BookingService();
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public AddReservationViewModel()
        {
            Habitaciones = new ObservableCollection<Room>();
            EditCommand = new RelayCommand(EditRoom);
            DeleteCommand = new RelayCommand(DeleteRoom);
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
                MessageBox.Show("No hay habitaciones disponibles para las fechas seleccionadas.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}