using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using HotelAsgard.Data;
using HotelAsgard.Models.Rooms;
using HotelAsgard.ViewModels;
using Newtonsoft.Json;

namespace HotelAsgard.Views.RoomsViews
{
    public partial class addRoom : Window
    {
        private readonly RoomService _roomService;
        private readonly AddRoomVM _viewModel;
        private Room _room;
        private List<string> _imagePaths = new List<string>();

        public addRoom(string titleText, string buttonText)
        {
            InitializeComponent();
            _roomService = new RoomService();
            _viewModel = new AddRoomVM();
            _room = new Room();
            
            DataContext = _viewModel;

            title.Text = titleText;
            this.Title = titleText;
            sendButton.Content = buttonText;

            // Obtener nuevo código desde la API
            LoadRoomData();
        }

        private async void LoadRoomData()
        {
            _room.Codigo = await _roomService.ObtenerNuevoCodigoAsync();
            roomCode.Text = _room.Codigo;
        }

        private void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Imágenes (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _imagePaths.AddRange(openFileDialog.FileNames);
                UpdateImageList();
            }
        }

        private void UpdateImageList()
        {
            ImageListBox.Items.Clear();

            if (_imagePaths.Count > 0)
            {
                // Mostrar la primera imagen como principal
                PreviewImage.Source = new BitmapImage(new Uri(_imagePaths[0]));

                // Agregar imágenes secundarias a la ListBox (excepto la primera)
                for (int i = 1; i < _imagePaths.Count; i++)
                {
                    ImageListBox.Items.Add(_imagePaths[i]);
                }
            }
            else
            {
                PreviewImage.Source = null;
            }
        }

        private void DeleteSelectedImage_Click(object sender, RoutedEventArgs e)
        {
            if (ImageListBox.SelectedIndex >= 0)
            {
                int selectedIndex = ImageListBox.SelectedIndex + 1; // +1 porque la principal no está en la lista

                if (selectedIndex < _imagePaths.Count)
                {
                    _imagePaths.RemoveAt(selectedIndex);
                    UpdateImageList();
                }
            }
        }

        private void SetAsMainImage_Click(object sender, RoutedEventArgs e)
        {
            if (ImageListBox.SelectedIndex >= 0)
            {
                int selectedIndex = ImageListBox.SelectedIndex + 1; // +1 porque la principal no está en la lista
                if (selectedIndex < _imagePaths.Count)
                {
                    // Intercambiar la imagen seleccionada con la principal
                    string selectedImage = _imagePaths[selectedIndex];
                    _imagePaths.RemoveAt(selectedIndex);
                    _imagePaths.Insert(0, selectedImage);

                    UpdateImageList();
                }
            }
        }

        private void DeleteAllImages_Click(object sender, RoutedEventArgs e)
        {
            _imagePaths.Clear();
            PreviewImage.Source = null; // Vaciar la imagen principal
            UpdateImageList();
        }
        
        private void roomCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (roomCategory.SelectedItem is Category categoriaSeleccionada)
            {
                // Depuración: Mostrar en consola qué categoría se ha seleccionado
                Console.WriteLine($"Categoría seleccionada: {categoriaSeleccionada.Nombre}");

                // Asignar los valores de la categoría a `_room`
                _room.Categoria = categoriaSeleccionada.Nombre;
                _room.Tamanyo = categoriaSeleccionada.Tamanyo;

                // 🔹 Se crea una nueva lista de camas para asegurarnos de que no hay referencias cruzadas
                _room.Camas = categoriaSeleccionada.Camas != null 
                    ? new List<Bed>(categoriaSeleccionada.Camas) 
                    : new List<Bed>();

                // 🔹 Se crea una nueva lista de servicios
                _room.Servicios = categoriaSeleccionada.Servicios != null 
                    ? new List<string>(categoriaSeleccionada.Servicios) 
                    : new List<string>();

                _room.NumPersonas = categoriaSeleccionada.NumPersonas;
                _room.Precio = categoriaSeleccionada.Precio;

                // // Actualizar UI con los valores de la categoría seleccionada
                // roomSize.Text = _room.Tamanyo.ToString();
                // maxGuests.Text = _room.NumPersonas.ToString();
                // roomPrice.Text = _room.Precio.ToString();

                // Depuración: Mostrar el número de camas y servicios en la consola
                Console.WriteLine($"Tamaño: {_room.Tamanyo}, NumPersonas: {_room.NumPersonas}, Precio: {_room.Precio}");
                Console.WriteLine($"Camas asignadas: {_room.Camas.Count}, Servicios asignados: {_room.Servicios.Count}");
            }
            else
            {
                Console.WriteLine("Error: No se pudo obtener la categoría seleccionada.");
            }
        }



        private async void SendButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Convertir la descripción de RichTextBox a texto plano
                TextRange textRange = new TextRange(DescriptionRichTextBox.Document.ContentStart, DescriptionRichTextBox.Document.ContentEnd);
                _room.Descripcion = textRange.Text.Trim();

                // Obtener valores de la UI
                _room.Nombre = roomName.Text;
                //_room.Categoria = roomCategory.Text;
                //_room.NumPersonas = int.Parse(maxGuests.Text);
                //_room.Precio = decimal.Parse(roomPrice.Text);
                _room.Habilitada = true;
                _room.Imagenes = new List<string>(_imagePaths);

                // ✅ Verificar si `Camas` y `Servicios` están vacíos y evitar enviar `null`
                if (_room.Camas == null)
                    _room.Camas = new List<Bed>();

                if (_room.Servicios == null)
                    _room.Servicios = new List<string>();

                // Depuración: Verificar los datos antes de enviarlos
                Console.WriteLine($"Enviando habitación: Código={_room.Codigo}, Nombre={_room.Nombre}, Categoría={_room.Categoria}");
                Console.WriteLine($"Tamaño={_room.Tamanyo}, NumPersonas={_room.NumPersonas}, Precio={_room.Precio}");
                Console.WriteLine($"Camas={JsonConvert.SerializeObject(_room.Camas)}, Servicios={JsonConvert.SerializeObject(_room.Servicios)}");
                Console.WriteLine($"Imágenes={_room.Imagenes.Count}");

                
                bool success = await _roomService.CrearHabitacionAsync(_room, _imagePaths);

                if (success)
                {
                    MessageBox.Show("Habitación creada con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    _imagePaths.Clear();
                    UpdateImageList();
                    searchRooms sr = new searchRooms();
                    sr.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error al crear la habitación. ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
    }
}