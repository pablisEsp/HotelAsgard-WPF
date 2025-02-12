using System.Net.Http;
using System.Windows.Documents;
using HotelAsgard.Models.Rooms;
using Newtonsoft.Json;

namespace HotelAsgard.Data;

public class RoomService
{
    private readonly HttpClient _httpClient;

    public RoomService()
    {
        _httpClient = new HttpClient();
    }
    
    public HttpClient API => _httpClient;
    

    public async Task<List<Room>> GetRooms() //function to get all rooms
    {
        try
        {
            string _urlEndpoint = "http://localhost:3000/api/rooms";

            var response = await API.GetAsync(_urlEndpoint);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var reservas = JsonConvert.DeserializeObject<List<Room>>(json);

            return reservas;
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException($"Error getting rooms from API: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}");
        }
    }
    
    public async Task<bool> ToggleRoomAvailability(Room room) // function to toggle availability
    {
        try
        {
            string url = $"http://localhost:3000/api/rooms/{room.Codigo}/toggle";
            var response = await _httpClient.PutAsync(url, null);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al cambiar el estado de la habitación: {ex.Message}");
        }
    }
    
    public async Task<List<Category>> GetCategorias() // function to get categories
    {
        try
        {
            string url = "http://localhost:3000/api/rooms/categories"; // Endpoint del backend
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var categorias = JsonConvert.DeserializeObject<List<Category>>(json);

            return categorias;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener las categorías: {ex.Message}");
        }
    }


    
}