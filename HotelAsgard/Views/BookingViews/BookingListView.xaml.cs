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
    /// Lógica de interacción para BookingListView.xaml
    /// </summary>
    public partial class BookingListView : Window
    {
        public BookingListView()
        {
            InitializeComponent();
            BookByRoom br = new BookByRoom();
            br.Show();

            AddReservation ad = new AddReservation();
            ad.Show();
            DataContext = new MainViewModel();
        }
    }

    public class MainViewModel
    {
        public ObservableCollection<Reserva> Reservas { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public MainViewModel()
        {
            Reservas = new ObservableCollection<Reserva>
            {
                new Reserva { Id = 1, Foto = "\"../../Images/hab_odin.jpg\"", Codigo = "ABC123", Rol = "Administrador", Email = "ivan@example.com" },
                new Reserva { Id = 2, Foto = "\"../../Images/hab_odin.jpg\"", Codigo = "DEF456", Rol = "Usuario", Email = "maria@example.com" },
                new Reserva { Id = 1, Foto = "\"../../Images/hab_odin.jpg\"", Codigo = "ABC123", Rol = "Administrador", Email = "ivan@example.com" },
                new Reserva { Id = 1, Foto = "\"../../Images/hab_odin.jpg\"", Codigo = "ABC123", Rol = "Administrador", Email = "ivan@example.com" },
                new Reserva { Id = 1, Foto = "\"../../Images/hab_odin.jpg\"", Codigo = "ABC123", Rol = "Administrador", Email = "ivan@example.com" },
            };

            EditCommand = new RelayCommand(EditReserva);
            DeleteCommand = new RelayCommand(DeleteReserva);
        }

        private void EditReserva(object obj)
        {
            if (obj is Reserva reserva)
            {
                MessageBox.Show($"Editando reserva: {reserva.Codigo}");
            }
        }

        private void DeleteReserva(object obj)
        {
            if (obj is Reserva reserva)
            {
                Reservas.Remove(reserva);
            }
        }
    }

    public class Reserva
    {
        public int Id { get; set; }
        public string Foto { get; set; }
        public string Codigo { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
