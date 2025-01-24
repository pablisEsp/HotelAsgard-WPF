using HotelAsgard.Views.BookingViews;
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
        private void BuscarUsuario_Click(object sender, RoutedEventArgs e)
        {

            SearchUserWindow searchUserWindow = new SearchUserWindow();
            searchUserWindow.Show();
            this.Close();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {

            AddUserWindow aw = new AddUserWindow();
            aw.Show();
            this.Close();
        }
        private void SearchBooking_Click(object sender, RoutedEventArgs e)
        {

            BookByRoom br = new BookByRoom();
            br.Show();
            this.Close();
        }


    }
}
