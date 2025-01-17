using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;


namespace HotelAsgard.Views.BookingViews
{
    /// <summary>
    /// Lógica de interacción para BookingListView.xaml
    /// </summary>
    public partial class BookingListView : Window
    {
        public BookingListView()
        {
            DataExamples dt = new DataExamples();

            UsuarioSingleton.ObtenerInstancia(usuario: dt.UsuarioActivo);

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
               // new Reserva { Id = 1, Foto = "\"../../Images/hab_odin.jpg\"", Codigo = "ABC123", Rol = "Administrador", Email = "ivan@example.com" },
                // new Reserva { Id = 2, Foto = "\"../../Images/hab_odin.jpg\"", Codigo = "DEF456", Rol = "Usuario", Email = "maria@example.com" },
                //new Reserva { Id = 1, Foto = "\"../../Images/hab_odin.jpg\"", Codigo = "ABC123", Rol = "Administrador", Email = "ivan@example.com" },
                // new Reserva { Id = 1, Foto = "\"../../Images/hab_odin.jpg\"", Codigo = "ABC123", Rol = "Administrador", Email = "ivan@example.com" },
                // new Reserva { Id = 1, Foto = "\"../../Images/hab_odin.jpg\"", Codigo = "ABC123", Rol = "Administrador", Email = "ivan@example.com" },
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
