﻿using System;
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

namespace HotelAsgard.Views.UserViews
{
    /// <summary>
    /// Lógica de interacción para SearchUserWindow.xaml
    /// </summary>
    public partial class SearchUserWindow : Window
    {
        public SearchUserWindow()
        {
            InitializeComponent();
            DataContext = new UsuarioViewModel();
        }
    }
}
