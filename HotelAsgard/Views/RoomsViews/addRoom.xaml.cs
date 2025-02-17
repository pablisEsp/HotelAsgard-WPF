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
            _room = new Room(); // Se crea una nueva habitación

            DataContext = _viewModel;

            title.Text = titleText;
            this.Title = titleText;
            sendButton.Content = buttonText;

            LoadNewRoomCode(); // Se obtiene un nuevo código de habitación

            _viewModel.IsReadOnlyMode = false; // Modo edición activado (botón "Enviar" visible)
        }


        public addRoom(string titleText, string buttonText, Room roomToEdit, bool isReadOnly = false)
        {
            InitializeComponent();
            _roomService = new RoomService();
            _viewModel = new AddRoomVM();
            _room = roomToEdit ??
                    new Room(); // Si `roomToEdit` es null, significa que estamos creando una nueva habitación

            DataContext = _viewModel;

            title.Text = titleText;
            this.Title = titleText;
            sendButton.Content = buttonText;

            if (roomToEdit != null)
            {
                LoadRoomData(roomToEdit); // Cargar datos de la habitación
            }
            else
            {
                LoadNewRoomCode(); // Si no hay habitación, crear una nueva
            }

            _viewModel.IsReadOnlyMode = isReadOnly; // Se define si es solo lectura o editable

            if (isReadOnly)
            {
                SetReadOnlyMode(); // Si es solo lectura, deshabilitar controles
            }
        }


        private async void LoadNewRoomCode()
        {
            _room.Codigo = await _roomService.ObtenerNuevoCodigoAsync();
            roomCode.Text = _room.Codigo;
        }

        private async void LoadRoomData(Room roomToEdit)
        {
            roomCode.Text = roomToEdit.Codigo;
            roomName.Text = roomToEdit.Nombre;

            // Asegurar que las categorías están cargadas antes de seleccionar una
            await Task.Delay(500); // Pequeña pausa para esperar la carga

            if (_viewModel.Categorias.Any())
            {
                _viewModel.CategoriaSeleccionada =
                    _viewModel.Categorias.FirstOrDefault(c => c.Nombre == roomToEdit.Categoria);
            }

            maxGuests.Text = roomToEdit.NumPersonas.ToString();
            roomPrice.Text = roomToEdit.Precio.ToString();

            if (!string.IsNullOrEmpty(roomToEdit.Descripcion))
            {
                DescriptionRichTextBox.Document.Blocks.Clear();
                DescriptionRichTextBox.Document.Blocks.Add(new Paragraph(new Run(roomToEdit.Descripcion)));
            }

            // Asegurar que las imágenes tengan una URL absoluta
            string baseUrl = "http://localhost:3000";

            if (roomToEdit.Imagenes != null && roomToEdit.Imagenes.Any())
            {
                _imagePaths = roomToEdit.Imagenes.Select(img =>
                    img.StartsWith("/") ? $"{baseUrl}{img}" : img
                ).ToList();

                UpdateImageList();
            }
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
                foreach (var file in openFileDialog.FileNames)
                {
                    if (!_imagePaths.Contains(file))
                    {
                        _imagePaths.Add(file);
                    }
                }

                UpdateImageList();
            }
        }

        private void UpdateImageList()
        {
            ImageListBox.Items.Clear();

            if (_imagePaths.Count > 0)
            {
                try
                {
                    PreviewImage.Source = new BitmapImage(new Uri(_imagePaths[0]));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error al cargar la imagen: {ex.Message}");
                }

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
            // Verificar si hay imágenes en la lista
            if (_imagePaths.Count == 0 || ImageListBox.SelectedIndex == -1)
            {
                return; // No hay imágenes o no hay ninguna seleccionada, no hacer nada
            }
            
            // Mostrar cuadro de diálogo de confirmación
            MessageBoxResult result = MessageBox.Show(
                "¿Estás seguro de que quieres eliminar la imagen seleccionada?", 
                "Confirmar eliminación", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Warning);

            // Si el usuario selecciona "Yes", proceder con la eliminación
            if (result == MessageBoxResult.Yes)
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
        }

        private void SetAsMainImage_Click(object sender, RoutedEventArgs e)
        {
            // Verificar si hay imágenes en la lista
            if (_imagePaths.Count == 0)
            {
                return; // No hay imágenes o no hay ninguna seleccionada, no hacer nada
            }
            
            if (ImageListBox.SelectedIndex >= 0)
            {
                int selectedIndex = ImageListBox.SelectedIndex + 1; // +1 porque la principal no está en la lista

                if (selectedIndex < _imagePaths.Count)
                {
                    // Mover la imagen seleccionada al primer lugar
                    string selectedImage = _imagePaths[selectedIndex];
                    _imagePaths.RemoveAt(selectedIndex);
                    _imagePaths.Insert(0, selectedImage);

                    UpdateImageList();
                }
            }
        }


        private void DeleteAllImages_Click(object sender, RoutedEventArgs e)
        {
            // Verificar si hay imágenes en la lista
            if (_imagePaths.Count == 0)
            {
                return; // No hay imágenes o no hay ninguna seleccionada, no hacer nada
            }
            
            // Mostrar cuadro de diálogo de confirmación
            MessageBoxResult result = MessageBox.Show(
                "¿Estás seguro de que quieres eliminar todas las imágenes?", 
                "Confirmar eliminación", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Warning);

            // Si el usuario selecciona "Yes", proceder con la eliminación
            if (result == MessageBoxResult.Yes)
            {
                _imagePaths.Clear();
                PreviewImage.Source = null; // Vaciar la imagen principal
                UpdateImageList();
            }
        }


        private void roomCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (roomCategory.SelectedItem is Category categoriaSeleccionada)
            {
                if (categoriaSeleccionada.Nombre == "Añadir nueva categoría") // Detectar la opción especial
                {
                    AbrirVentanaNuevaCategoria();
                }
                else
                {
                    // Asignar los valores de la categoría seleccionada a la habitación
                    _room.Categoria = categoriaSeleccionada.Nombre;
                    _room.Tamanyo = categoriaSeleccionada.Tamanyo;

                    // Se crea una nueva lista de camas para evitar referencias cruzadas
                    _room.Camas = categoriaSeleccionada.Camas != null
                        ? new List<Bed>(categoriaSeleccionada.Camas)
                        : new List<Bed>();

                    // Se crea una nueva lista de servicios
                    _room.Servicios = categoriaSeleccionada.Servicios != null
                        ? new List<string>(categoriaSeleccionada.Servicios)
                        : new List<string>();

                    _room.NumPersonas = categoriaSeleccionada.NumPersonas;
                    _room.Precio = categoriaSeleccionada.Precio;

                    // Depuración: Mostrar en consola los datos seleccionados
                    Console.WriteLine($"Categoría seleccionada: {_room.Categoria}");
                    Console.WriteLine(
                        $"Tamaño: {_room.Tamanyo}, NumPersonas: {_room.NumPersonas}, Precio: {_room.Precio}");
                    Console.WriteLine($"Camas: {_room.Camas.Count}, Servicios: {_room.Servicios.Count}");
                }
            }
        }

        private void AbrirVentanaNuevaCategoria()
        {
            NewCategory ventanaNuevaCategoria = new NewCategory();
            if (ventanaNuevaCategoria.ShowDialog() == true) // Si el usuario presionó "Guardar"
            {
                Category nuevaCategoria = new Category
                {
                    Nombre = ventanaNuevaCategoria.NuevaCategoria.Nombre,
                    Tamanyo = ventanaNuevaCategoria.NuevaCategoria.Tamanyo,
                    NumPersonas = ventanaNuevaCategoria.NuevaCategoria.NumPersonas,
                    Precio = ventanaNuevaCategoria.NuevaCategoria.Precio,
                    Camas = ventanaNuevaCategoria.NuevaCategoria.Camas,
                    Servicios = ventanaNuevaCategoria.NuevaCategoria.Servicios
                };

                _viewModel.AgregarNuevaCategoria(nuevaCategoria);
            }
            else
            {
                // Si el usuario cancela, evitar que quede seleccionada la opción de "Añadir nueva categoría"
                roomCategory.SelectedIndex = -1;
            }
        }

        
        private async void SendButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                searchRooms searchRoom;

                TextRange textRange = new TextRange(DescriptionRichTextBox.Document.ContentStart,
                    DescriptionRichTextBox.Document.ContentEnd);
                _room.Descripcion = textRange.Text.Trim();

                _room.Nombre = roomName.Text;
                _room.Categoria = ((Category)roomCategory.SelectedItem)?.Nombre;
                _room.NumPersonas = int.Parse(maxGuests.Text);
                _room.Precio = decimal.Parse(roomPrice.Text);
                _room.Imagenes = new List<string>(_imagePaths);
                _room.Habilitada = true;
                
                // Verificar que la descripción y el nombre no estén vacíos
                if (string.IsNullOrWhiteSpace(_room.Descripcion) || string.IsNullOrWhiteSpace(_room.Nombre))
                {
                    MessageBox.Show("El nombre y/o la descripción de la habitación no pueden estar vacíos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Verificar si hay al menos 1 imagen
                if (_room.Imagenes.Count < 1)
                {
                    MessageBox.Show("Debes añadir al menos 1 imagen a la habitación.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Verificar si hay más de 5 imágenes
                if (_room.Imagenes.Count > 5)
                {
                    MessageBox.Show("No puedes añadir más de 5 imágenes a la habitación.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                bool success;

                // Verificamos si la habitación YA EXISTE en la base de datos
                bool habitacionExiste = await _roomService.RoomExists(_room.Codigo);

                if (habitacionExiste)
                {
                    // Si la habitación ya existe en la base de datos, la actualizamos
                    success = await _roomService.ActualizarHabitacionAsync(_room, _imagePaths);
                }
                else
                {
                    // Si la habitación no existe, creamos una nueva
                    success = await _roomService.CrearHabitacionAsync(_room, _imagePaths);
                }

                searchRoom = new searchRooms();
                if (success)
                {
                    MessageBox.Show("Habitación guardada con éxito.", "Éxito", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    searchRoom.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error al guardar la habitación.", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    searchRoom.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void GoBackButton(object sender, RoutedEventArgs e)
        {
            searchRooms sr = new searchRooms();
            sr.Show();
            this.Close();
        }

        private void SetReadOnlyMode()
        {
            roomName.IsReadOnly = true;
            roomCategory.IsEnabled = false;
            maxGuests.IsReadOnly = true;
            roomPrice.IsReadOnly = true;

            // Deshabilitar la edición de la descripción
            DescriptionRichTextBox.IsReadOnly = true;

            // Deshabilitar botones de imágenes
            UploadImageButton.IsEnabled = false;
            DeleteSelectedImage.IsEnabled = false;
            SetAsMainImage.IsEnabled = false;
            DeleteAllImages.IsEnabled = false;

            // Deshabilitar el botón de guardar
            sendButton.Visibility = Visibility.Collapsed;
        }
    }
}