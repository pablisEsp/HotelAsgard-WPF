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
    

    public async Task<List<Room>> GetRooms()
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
    
}