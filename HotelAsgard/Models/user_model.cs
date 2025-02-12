public class Usuario
{
    public Usuario(int id, string nombre, string password, string apellido1, string apellido2, string dNIPasaporte, string movil, string fechaNacimiento, string fechaRegistro, string tipo, List<string> reservas, List<string> imagenes)
    {
        Id = id;
        Nombre = nombre;
        Password = password;
        Apellido1 = apellido1;
        Apellido2 = apellido2;
        DNIPasaporte = dNIPasaporte;
        Movil = movil;
        FechaNacimiento = fechaNacimiento;
        FechaRegistro = fechaRegistro;
        Tipo = tipo;
        Reservas = reservas;
        Imagenes = imagenes;
    }

    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Password { get; set; }
    public string Apellido1 { get; set; }
    public string Apellido2 { get; set; }
    public string DNIPasaporte { get; set; }
    public string Movil { get; set; }
    public string FechaNacimiento { get; set; } // Manejado como string temporalmente
    public string FechaRegistro { get; set; } // Manejado como string temporalmente
    public string Tipo { get; set; }
    public List<string> Reservas { get; set; } // Almacena IDs de reservas asociadas
    public List<string> Imagenes { get; set; } // Almacena rutas o URLs de imágenes

}

