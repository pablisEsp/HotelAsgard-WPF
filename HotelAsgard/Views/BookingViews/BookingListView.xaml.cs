using HotelAsgard.Views.UserViews;
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
            InitializeComponent();
            DataExamples dt = new DataExamples();

            UsuarioSingleton.ObtenerInstancia(usuario: dt.UsuarioActivo);

            DataContext = new MainViewModel();
            
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

        private void MainWindows_click(object sender, RoutedEventArgs e)
        {
            initial_view iv = new initial_view();
            iv.Show();
            this.Close();
        }
        private void Perfil_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para la opción "Perfil"
            AddUserWindow addUser = new AddUserWindow();
            addUser.Show();
            this.Close();
        }

        private void BuscarUsuario_Click(object sender, RoutedEventArgs e)
        {
            SearchUserWindow searchUser = new SearchUserWindow();
            searchUser.Show();
            this.Close();

        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUser = new AddUserWindow();
            addUser.Show();
            this.Close();
        }

        private void SearchBooking_Click(object sender, RoutedEventArgs e)
        {
            AddReservation addReservation = new AddReservation();
            addReservation.Show();
            this.Close();
        }

        private void AddBooking_Click(object sender, RoutedEventArgs e)
        {
            BookByRoom bookByRoom = new BookByRoom();
            bookByRoom.Show();
            this.Close();
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