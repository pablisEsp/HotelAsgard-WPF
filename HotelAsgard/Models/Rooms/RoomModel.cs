using System.Text.Json.Serialization;

namespace HotelAsgard.Models.Rooms
{
    
    public class Room
    {
        public string _id { get; set; }
        
        public string Codigo { get; set; }
        
        public string Nombre { get; set; }
        
        public string Categoria { get; set; }
        
        public List<Bed> Camas { get; set; } = new List<Bed>();

        
        public int Tamanyo { get; set; }

        
        public List<string> Servicios { get; set; } = new List<string>();
        
        public int NumPersonas { get; set; }
        
        public decimal Precio { get; set; }
        
        public string Descripcion { get; set; }

        public List<string> Imagenes { get; set; } = new List<string>();

        public bool Habilitada { get; set; }
    }
}

