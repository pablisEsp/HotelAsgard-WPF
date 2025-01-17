
public class Reserva
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public string FechaInicio { get; set; } // Manejado como string temporalmente
    public string FechaFin { get; set; } // Manejado como string temporalmente
    public int CodigoHabitacion { get; set; } // Referencia al ID de la habitación reservada
}