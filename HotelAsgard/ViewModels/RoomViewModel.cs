using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HotelAsgard.Data;
using HotelAsgard.Models.Rooms;

namespace HotelAsgard.ViewModels
{
    public class RoomViewModel : INotifyPropertyChanged
    {
        private readonly RoomService _roomService;
        private ObservableCollection<Room> _rooms;

        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set
            {
                _rooms = value;
                OnPropertyChanged();
            }
        }

        public RoomViewModel()
        {
            _roomService = new RoomService();
            _rooms = new ObservableCollection<Room>();
            LoadRooms();
        }

        private async void LoadRooms()
        {
            try
            {
                var roomsList = await _roomService.GetRooms();
                Rooms = new ObservableCollection<Room>(roomsList);
            }
            catch (Exception ex)
            {
                // Manejo de errores, puedes mostrarlo en un log o en la UI
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
