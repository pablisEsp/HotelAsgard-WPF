namespace HotelAsgard.Models;

public class Root
{
    public Usuario usuario { get; set; }
}

public class Usuario
{
    public string _id { get; set; }
    public string Nombre { get; set; }
    public string Apellido1 { get; set; }
    public string Apellido2 { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string DniPasaporte { get; set; }
    public string Movil { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Tipo { get; set; }
    public List<Reserva> Reservas { get; set; }
    public DateTime FechaRegistro { get; set; }
}

