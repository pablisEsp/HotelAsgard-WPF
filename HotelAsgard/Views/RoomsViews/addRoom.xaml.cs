using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using HotelAsgard.Models.Rooms;
using HotelAsgard.ViewModels;

namespace HotelAsgard.Views.RoomsViews
{
    /// <summary>
    /// Lógica de interacción para addRoom.xaml
    /// </summary>
    public partial class addRoom : Window
    {
        Room r = new Room();
        private List<string> imagePaths = new List<string>();


        public addRoom(string titleText, string buttonText)
        {
            InitializeComponent();
            title.Text = titleText;
            this.Title = titleText;
            DataContext = new AddRoomVM();

            sendButton.Content = buttonText;
        }

        public addRoom(string titleText, string buttonText, string name, int guests, string description, bool cradle,
            bool extraBed, int price, bool info)
        {
            if (info)
            {
                roomName.IsReadOnly = true;
                maxGuests.IsReadOnly = true;
                DescriptionRichTextBox.IsReadOnly = true;
                //cradleCheck.IsEnabled = false;
                //extraBedCheck.IsEnabled = false;
                roomPrice.IsReadOnly = true;
                sendButton.Visibility = Visibility.Collapsed;
            }

            InitializeComponent();
            title.Text = titleText;
            this.Title = titleText;
            roomName.Text = name;
            //maxGuests.SelectedIndex = guests-1;

            // access the flowdocument content as
            FlowDocument flowDoc = DescriptionRichTextBox.Document;

            // create new content
            flowDoc.Blocks.Clear(); // clear previous content
            flowDoc.Blocks.Add(new Paragraph(new Run(description)));


            //cradleCheck.IsChecked = cradle;
            //extraBedCheck.IsChecked = cradle;
            roomPrice.Text = price + "";

            sendButton.Content = buttonText;
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
                imagePaths.AddRange(openFileDialog.FileNames);
                UpdateImageList();
            }
        }

        private void UpdateImageList()
        {
            ImageListBox.Items.Clear();

            if (imagePaths.Count > 0)
            {
                // Mostrar la primera imagen como principal
                PreviewImage.Source = new BitmapImage(new Uri(imagePaths[0]));

                // Agregar imágenes secundarias a la ListBox (excepto la primera)
                for (int i = 1; i < imagePaths.Count; i++)
                {
                    ImageListBox.Items.Add(imagePaths[i]);
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

                if (selectedIndex < imagePaths.Count)
                {
                    imagePaths.RemoveAt(selectedIndex);
                    UpdateImageList();
                }
            }
        }

        private void SetAsMainImage_Click(object sender, RoutedEventArgs e)
        {
            if (ImageListBox.SelectedIndex >= 0)
            {
                int selectedIndex = ImageListBox.SelectedIndex + 1; // +1 porque la principal no está en la lista
                if (selectedIndex < imagePaths.Count)
                {
                    // Intercambiar la imagen seleccionada con la principal
                    string selectedImage = imagePaths[selectedIndex];
                    imagePaths.RemoveAt(selectedIndex);
                    imagePaths.Insert(0, selectedImage);

                    UpdateImageList();
                }
            }
        }

        private void DeleteAllImages_Click(object sender, RoutedEventArgs e)
        {
            imagePaths.Clear();
            PreviewImage.Source = null; // Vaciar la imagen principal
            UpdateImageList();
        }
    }
}