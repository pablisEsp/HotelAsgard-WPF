
using HotelAsgard.Models.Rooms;

namespace HotelAsgard.Models;

public class Reserva
{
    public string Codigo { get; set; } = null!;
    public DateTime? FechaInicio { get; set; } = null!;
    public DateTime? FechaFin { get; set; } =null!;
    public Room Habitacion { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
    public string Precio { get; set; } = null!;
}