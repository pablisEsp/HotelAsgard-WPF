﻿using HotelAsgard.Views.BookingViews;
using HotelAsgard.Views.RoomsViews;
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

            BookingListView bookingListView = new BookingListView();
            bookingListView.Show();
            this.Close();
        }

        private void AddBooking_Click(object sender, RoutedEventArgs e)
        {

            AddReservation addReservation= new AddReservation();
            addReservation.Show();
            this.Close();
        }

        private void SearchRooms_Click(object sender, RoutedEventArgs e)
        {
            searchRooms sr = new searchRooms();
            sr.Show();
            this.Close();
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            addRoom ar = new addRoom("Añadir habitación", "Añadir");
            ar.Show();
            this.Close();

        }
    }
}
