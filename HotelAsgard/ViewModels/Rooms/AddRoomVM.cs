using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using HotelAsgard.Data;
using HotelAsgard.Models.Rooms;

namespace HotelAsgard.ViewModels
{
    public class AddRoomVM : INotifyPropertyChanged
    {
        private readonly RoomService _roomService;
        private ObservableCollection<Category> _categorias;
        private Category _categoriaSeleccionada;

        public ObservableCollection<Category> Categorias
        {
            get => _categorias;
            set
            {
                _categorias = value;
                OnPropertyChanged(); // Notifica que la colección ha cambiado
            }
        }

        public Category CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set
            {
                _categoriaSeleccionada = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Precio));
                OnPropertyChanged(nameof(NumPersonas));
                OnPropertyChanged(nameof(Camas));
            }
        }

        public int Precio => CategoriaSeleccionada?.Precio ?? 0;
        public int NumPersonas => CategoriaSeleccionada?.NumPersonas ?? 0;
        public List<Bed> Camas => CategoriaSeleccionada?.Camas ?? new List<Bed>();

        public AddRoomVM()
        {
            _roomService = new RoomService();
            Categorias = new ObservableCollection<Category>();
            _ = LoadCategorias();
        }

        private async Task LoadCategorias()
        {
            try
            {
                var categoriasList = await _roomService.GetCategorias();

                // Asegurar que la actualización se haga en la UI principal
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Categorias.Clear();
                    foreach (var categoria in categoriasList)
                    {
                        Categorias.Add(categoria);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar categorías: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
