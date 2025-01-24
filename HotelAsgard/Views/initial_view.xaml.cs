using HotelAsgard.Views.UserViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelAsgard.Views
{
    /// <summary>
    /// Lógica de interacción para initial_view.xaml
    /// </summary>
    public partial class initial_view : Window
    {
        public initial_view()
        {
            InitializeComponent();
        }

        private void Perfil_Click(object sender, RoutedEventArgs e)
        {
            
            AddUserWindow aw = new AddUserWindow();
            aw.Show();
            this.Close();
        }
    }
}
