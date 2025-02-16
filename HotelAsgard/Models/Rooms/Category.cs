using Newtonsoft.Json;

namespace HotelAsgard.Models.Rooms;

public class Category
{
    [JsonProperty("_id")]
    public string Id { get; set; }        // El "_id" de MongoDB
    public string Nombre
    {
        get => Id;
        set => Id = value;
    }

    public int Precio { get; set; }       // Precio de la categoría
    public int NumPersonas { get; set; }  // Capacidad de huéspedes
    
    [JsonProperty("camas")]
    public List<Bed> Camas { get; set; }  // Lista de camas
    public int Tamanyo { get; set; } 
    public List<string> Servicios { get; set; }
}