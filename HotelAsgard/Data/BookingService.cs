using System.Net.Http;
using System.Text;
using System.Web;
using System.Windows;
using HotelAsgard.Models;
using HotelAsgard.Models.Rooms;
using Newtonsoft.Json;

namespace HotelAsgard.Data;

public class BookingService
{
    
    private readonly HttpClient _httpClient;

    public BookingService()
    {
        _httpClient = new HttpClient();
    }
    
    public HttpClient API => _httpClient;
    
    
    
    public async Task<List<Reserva>?> GetBookings()
    {
        try
        {
            String urlEndpoint = Constants._baseUrl+"/api/bookings/getBookings";

            var response = await API.GetAsync(urlEndpoint);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var reservas = JsonConvert.DeserializeObject<List<Reserva>>(json);

            return reservas;
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show(ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return null;
        }
    } 
    
 
    public async Task<List<Reserva>?> SearchBookings(string? fechaInicio = null, string? fechaFin = null, string? codigo = null, string? nombre = null, string? tipo = null)
    {
        try
        {
            string baseUrl = Constants._baseUrl; // URL base de la API
            string endpoint = "/api/bookings/searchBookings";
            string fullUrlEndpoint = baseUrl + endpoint;

            var requestBody = new
            {
                fechaInicio,
                fechaFin,
                codigo,
                nombre,
                tipo
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await API.PostAsync(fullUrlEndpoint, jsonContent);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var reservas = JsonConvert.DeserializeObject<List<Reserva>>(json);

            return reservas;
        }
        catch (HttpRequestException ex)
        {
            //MessageBox.Show("No se han encontrado reservas con esos parametros", "Error");
            throw;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Exception: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Usuario>>? GetUsers()
    {  try
        {
            String urlEndpoint = Constants._baseUrl+"/api/bookings/getUsers";

            var response = await API.GetAsync(urlEndpoint);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);

            return usuarios;
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show(ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return null;
        }
        
    }
    
    public async Task<List<Room>?> SearchFreeRooms(string fechaInicio, string fechaFin, int numeroHuespedes)
    {
        try
        {
            string baseUrl = Constants._baseUrl; 
            string endpoint = "/api/bookings/getFreeRoomsWPF";
            string fullUrlEndpoint = baseUrl + endpoint;

            //tengo que factorizar la llamada porque estoy aprovechando la de android
            string startdate = fechaInicio;
            string enddate = fechaFin;
            int numGuest = numeroHuespedes;
            
            var requestBody = new
            {
                startdate,
                enddate,
                numGuest
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await API.PostAsync(fullUrlEndpoint, jsonContent);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var habitaciones = JsonConvert.DeserializeObject<List<Room>>(json);

            return habitaciones;
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show("No se han encontrado habitaciones disponibles con esos parámetros.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return null;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }
    }
    
    
    
}
