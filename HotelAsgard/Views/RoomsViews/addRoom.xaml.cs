using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelAsgard.Views.RoomsViews
{
    /// <summary>
    /// Lógica de interacción para addRoom.xaml
    /// </summary>
    public partial class addRoom : Window
    {
        public addRoom(string titleText, string buttonText)
        {
            InitializeComponent();
            title.Text = titleText;
            this.Title = titleText;

            sendButton.Content = buttonText;
            
            
        }

        public addRoom(string titleText, string buttonText, string name, int guests, string description, bool cradle, bool extraBed, int price)
        {
            InitializeComponent();
            title.Text = titleText;
            this.Title = titleText;
            roomName.Text = name;
            maxGuests.SelectedIndex = guests-1;

            // access the flowdocument content
            FlowDocument flowDoc = DescriptionRichTextBox.Document;

            // create new content
            flowDoc.Blocks.Clear(); // clear previous content
            flowDoc.Blocks.Add(new Paragraph(new Run(description)));
            

            cradleCheck.IsChecked = cradle;
            extraBedCheck.IsChecked = cradle;
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
    }


}
