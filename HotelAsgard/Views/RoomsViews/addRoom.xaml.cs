using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using HotelAsgard.Models.Rooms;

namespace HotelAsgard.Views.RoomsViews
{
    /// <summary>
    /// Lógica de interacción para addRoom.xaml
    /// </summary>
    public partial class addRoom : Window
    {
        Room r = new Room();
        
        public addRoom(string titleText, string buttonText)
        {
            InitializeComponent();
            title.Text = titleText;
            this.Title = titleText;

            sendButton.Content = buttonText;
            
            
        } 

        public addRoom(string titleText, string buttonText, string name, int guests, string description, bool cradle, bool extraBed, int price, bool info)
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
            // Crear un diálogo para seleccionar archivos
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos de imagen (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp", // Filtro para imágenes
                Title = "Seleccionar una imagen"
            };

            // Mostrar el diálogo
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Cargar la imagen seleccionada en el control Image
                    BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                    //PreviewImage.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RoomCategory_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        
        
    }


}
