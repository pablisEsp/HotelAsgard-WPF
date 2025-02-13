
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using HotelAsgard.Models;

namespace HotelAsgard.ViewModels
{
    public class BookingListViewModel
    {
        public ObservableCollection<Reserva> Reservas { get; set; }
        
        
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public BookingListViewModel()
        {
            Reservas = new ObservableCollection<Reserva>();
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
}