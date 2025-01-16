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
    /// Lógica de interacción para editRoom.xaml
    /// </summary>
    public partial class editRoom : Window
    {
        public editRoom()
        {
            InitializeComponent();
        }

        private void UploadImageButtonEdit_Click(object sender, RoutedEventArgs e)
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
