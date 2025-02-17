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
    public class RoomVM : INotifyPropertyChanged
    {
        private readonly RoomService _roomService;
        private ObservableCollection<Room> _rooms;
        
        private ObservableCollection<Category> _categorias;
        private Category _categoriaSeleccionada;
        
        private int? _numPersonasMax;
        

        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set
            {
                _rooms = value;
                OnPropertyChanged();
            }
        }
        
        public ObservableCollection<Category> Categorias
        {
            get => _categorias;
            set { _categorias = value; OnPropertyChanged(); }
        }

        public Category CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set { _categoriaSeleccionada = value; OnPropertyChanged(); }
        }

        public int? NumPersonasMax
        {
            get => _numPersonasMax;
            set
            {
                _numPersonasMax = value;
                OnPropertyChanged();
            }
        }
        
        public RoomVM()
        {
            _roomService = new RoomService();
            _rooms = new ObservableCollection<Room>();
            _categorias = new ObservableCollection<Category>();

            LoadRooms();
            _ = LoadCategorias(); // Cargar las categorías al iniciar
            
            
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
                MessageBox.Show($"Ha ocurrido un error inesperado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async Task LoadCategorias()
        {
            try
            {
                var categoriasList = await _roomService.GetCategorias();
                Categorias.Clear();
                Categorias.Add(new Category { Id = "", Precio = 0, NumPersonas = 0 }); // Opción vacía para "sin filtro"
                foreach (var categoria in categoriasList)
                {
                    Categorias.Add(categoria);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ha ocurrido un error inesperado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task SearchRoomsAsync()
        {
            try
            {
                Console.WriteLine($"📌 Código: {RoomCode}");
                Console.WriteLine($"📌 Nombre: {RoomName}");
                Console.WriteLine($"📌 Categoría: {CategoriaSeleccionada?.Nombre}");
                Console.WriteLine($"📌 Número de Personas: {NumPersonasMax}");
                Console.WriteLine($"📌 Tamaño Min: {InitialSize}");
                Console.WriteLine($"📌 Tamaño Max: {FinalSize}");
                Console.WriteLine($"📌 Precio Min: {InitialPrice}");
                Console.WriteLine($"📌 Precio Max: {FinalPrice}");
                Console.WriteLine($"📌 Habilitada: {IsRoomUsable}");
                
                var habitaciones = await _roomService.SearchRooms(
                    RoomCode, 
                    RoomName, 
                    CategoriaSeleccionada?.Nombre, 
                    NumPersonasMax, 
                    InitialSize, 
                    FinalSize, 
                    InitialPrice, 
                    FinalPrice, 
                    IsRoomUsable
                );

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Rooms.Clear(); // Limpiar la lista antes de agregar nuevas habitaciones
                    foreach (var habitacion in habitaciones)
                    {
                        Rooms.Add(habitacion);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la búsqueda: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Propiedades para los filtros
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        //public int? NumPersonasSeleccionadas { get; set; }
        public int? InitialSize { get; set; }
        public int? FinalSize { get; set; }
        public decimal? InitialPrice { get; set; }
        public decimal? FinalPrice { get; set; }
        private bool _isRoomUsable = true; // Inicializado en true

        public bool IsRoomUsable
        {
            get => _isRoomUsable;
            set
            {
                if (_isRoomUsable != value)
                {
                    _isRoomUsable = value;
                    OnPropertyChanged(nameof(IsRoomUsable));
                }
            }
        }

        
        
        public void ClearFilters()
        {
            RoomCode = string.Empty;
            RoomName = string.Empty;
            CategoriaSeleccionada = null;
            NumPersonasMax = null;
            InitialSize = null;
            FinalSize = null;
            InitialPrice = null;
            FinalPrice = null;
            IsRoomUsable = true;

            OnPropertyChanged(nameof(RoomCode));
            OnPropertyChanged(nameof(RoomName));
            OnPropertyChanged(nameof(CategoriaSeleccionada));
            OnPropertyChanged(nameof(NumPersonasMax));
            OnPropertyChanged(nameof(InitialSize));
            OnPropertyChanged(nameof(FinalSize));
            OnPropertyChanged(nameof(InitialPrice));
            OnPropertyChanged(nameof(FinalPrice));
            OnPropertyChanged(nameof(IsRoomUsable));
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
