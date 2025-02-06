using System.Collections.ObjectModel;
using System.ComponentModel;
using HotelAsgard.Data;
using HotelAsgard.Models.Rooms;

namespace HotelAsgard.ViewModels
{
    public class RoomViewModel : INotifyPropertyChanged
    {
        private readonly RoomService _roomService = new RoomService();
        public ObservableCollection<Room> Rooms { get; set; } = new ObservableCollection<Room>();

        public event PropertyChangedEventHandler PropertyChanged;

        public RoomViewModel()
        {
            _ = LoadRoomsAsync(); // Loads all rooms at the start
        }

        private async Task LoadRoomsAsync()
        {
            var rooms = await _roomService.GetRooms();
            Rooms.Clear();

            foreach (var room in rooms)
            {
                Rooms.Add(room); // UI se actualiza automáticamente
            }
        }
    }
}
