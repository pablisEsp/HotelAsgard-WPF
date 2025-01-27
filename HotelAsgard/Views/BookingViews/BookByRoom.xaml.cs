using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace HotelAsgard.Views.BookingViews
{
    /// <summary>
    /// Lógica de interacción para BookByRoom.xaml
    /// </summary>
    public partial class BookByRoom : Window
    {
        public BookByRoom()
        {
            InitializeComponent();

        }

        private void DatePicker_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true; // Cancela la entrada de texto
        }

        // Evita que el usuario use el teclado para editar el texto
        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Permite solo teclas de navegación (flechas, tab, etc.)
            if (e.Key != Key.Enter && e.Key != Key.Tab && e.Key != Key.Escape)
            {
                e.Handled = true; // Cancela la entrada de teclado
            }
        }

        // Abre el calendario cuando el DatePicker recibe el foco
        private void DatePicker_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender == fechaInicio)
            {
                fechaInicio.IsDropDownOpen = true;

            }
            else
            {
                fechaFin.IsDropDownOpen = true;
            }
        }
    }
    


}
