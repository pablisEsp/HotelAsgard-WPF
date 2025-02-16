using Newtonsoft.Json;

namespace HotelAsgard.Models.Rooms
{
    public class Bed
    {
        [JsonIgnore] // Evita que _id se serialice
        public string Id { get; set; }
        
        [JsonProperty("numCamas")] // Mapea correctamente con el JSON
        public int NumCamas { get; set; }

        [JsonProperty("nombre")] // Mapea correctamente con el JSON
        public string Nombre { get; set; }
    }
}