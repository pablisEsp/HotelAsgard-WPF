public class Habitacion
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Categoria { get; set; }
    public int Camas { get; set; }
    public double Tamano { get; set; } // Tamaño en metros cuadrados
    public int Planta { get; set; }
    public List<string> Accesorios { get; set; } // Lista de accesorios disponibles
    public int NumPersonas { get; set; } // Capacidad máxima de personas
    public decimal Precio { get; set; } // Precio por noche
    public string Descripcion { get; set; } // Descripción de la habitación
    public List<string> Imagenes { get; set; } // Almacena rutas o URLs de imágenes
}

