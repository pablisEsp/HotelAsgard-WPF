using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using HotelAsgard.Data;
using HotelAsgard.Models.Rooms;
using Microsoft.Win32;

namespace HotelAsgard.ViewModels
{
    public class AddRoomVM : INotifyPropertyChanged
    {
        private readonly RoomService _roomService;
        private ObservableCollection<Category> _categorias;
        private Category _categoriaSeleccionada;
        private ObservableCollection<string> _imagenes;
        private string _codigo;
        
        private bool _isReadOnlyMode;
        
        public ObservableCollection<string> Imagenes
        {
            get => _imagenes;
            set
            {
                _imagenes = value;
                OnPropertyChanged();
            }
        }

        public string Codigo
        {
            get => _codigo;
            set
            {
                _codigo = value;
                OnPropertyChanged();
            }
        }

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
        
        public bool IsReadOnlyMode
        {
            get => _isReadOnlyMode;
            set
            {
                if (_isReadOnlyMode != value)
                {
                    _isReadOnlyMode = value;
                    OnPropertyChanged(nameof(IsReadOnlyMode));
                    OnPropertyChanged(nameof(IsEditMode)); // Notificar cambio en IsEditMode
                }
            }
        }

        public bool IsEditMode => !IsReadOnlyMode;

        
        public int Precio => CategoriaSeleccionada?.Precio ?? 0;
        public int NumPersonas => CategoriaSeleccionada?.NumPersonas ?? 0;
        public List<Bed> Camas => CategoriaSeleccionada?.Camas ?? new List<Bed>();

        public AddRoomVM()
        {
            _roomService = new RoomService();
            Categorias = new ObservableCollection<Category>();
            _ = LoadCategorias();
            Imagenes = new ObservableCollection<string>();

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
        
        public async Task<string> ObtenerNuevoCodigo()
        {
            Codigo = await _roomService.ObtenerNuevoCodigoAsync();
            return Codigo;
        }

        /// <summary>
        /// Envía los datos de la habitación al backend para crearla.
        /// </summary>
        public async Task<bool> CrearHabitacion(Room room)
        {
            bool success = await _roomService.CrearHabitacionAsync(room, new List<string>(Imagenes));

            if (success)
            {
                MessageBox.Show("Habitación creada con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            else
            {
                MessageBox.Show("Error al crear la habitación.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        
        public void AgregarImagen()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Imágenes (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    Imagenes.Add(file);
                }
            }
        }
        
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
