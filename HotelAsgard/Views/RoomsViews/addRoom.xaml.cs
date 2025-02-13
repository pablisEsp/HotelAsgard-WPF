using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using HotelAsgard.Data;
using HotelAsgard.Models.Rooms;
using HotelAsgard.ViewModels;
using Newtonsoft.Json;

namespace HotelAsgard.Views.RoomsViews
{
    /// <summary>
    /// Lógica de interacción para addRoom.xaml
    /// </summary>
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

        private async void SendButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Convertir la descripción de RichTextBox a texto plano
                TextRange textRange = new TextRange(DescriptionRichTextBox.Document.ContentStart, DescriptionRichTextBox.Document.ContentEnd);
                _room.Descripcion = textRange.Text.Trim();

                // Obtener valores de la UI
                _room.Nombre = roomName.Text;
                _room.Categoria = roomCategory.Text;
                _room.NumPersonas = int.Parse(maxGuests.Text);
                _room.Precio = decimal.Parse(roomPrice.Text);
                _room.Habilitada = true;
                _room.Imagenes = new List<string>(_imagePaths);

                // Convertir camas y servicios en JSON
                string camasJson = JsonConvert.SerializeObject(new List<Bed>()); // Puedes cambiarlo por la lista real de camas
                string serviciosJson = JsonConvert.SerializeObject(new List<string>()); // Puedes cambiarlo por la lista real de servicios

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
                    MessageBox.Show("Error al crear la habitación.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}