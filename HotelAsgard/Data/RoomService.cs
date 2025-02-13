using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
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
    
    public async Task<string> ObtenerNuevoCodigoAsync()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:3000/api/rooms/newCode");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                return data.ContainsKey("codigo") ? data["codigo"] : "HAB000";
            }
            return "HAB000";
        }
        catch
        {
            MessageBox.Show("Error al obtener el nuevo código de habitación.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return "HAB000";
        }
    }
    
    public async Task<bool> CrearHabitacionAsync(Room room, List<string> imagePaths)
        {
            try
            {
                var formData = new MultipartFormDataContent();

                // Datos básicos de la habitación
                formData.Add(new StringContent(room.Codigo), "codigo");
                formData.Add(new StringContent(room.Nombre), "nombre");
                formData.Add(new StringContent(room.Categoria), "categoria");
                formData.Add(new StringContent(room.Tamanyo.ToString()), "tamanyo");
                formData.Add(new StringContent(room.NumPersonas.ToString()), "numPersonas");
                formData.Add(new StringContent(room.Precio.ToString()), "precio");
                formData.Add(new StringContent(room.Descripcion), "descripcion");
                formData.Add(new StringContent(room.Habilitada ? "true" : "false"), "habilitada");

                // ✅ SOLUCIÓN: Serializar `camas` y `servicios` con Newtonsoft.Json
                formData.Add(new StringContent(JsonConvert.SerializeObject(room.Camas)), "camas");
                formData.Add(new StringContent(JsonConvert.SerializeObject(room.Servicios)), "servicios");

                // Adjuntar imágenes
                foreach (var imagePath in imagePaths)
                {
                    var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    formData.Add(imageContent, "imagenes", Path.GetFileName(imagePath));
                }

                // Hacer la petición POST
                HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:3000/api/rooms/", formData);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la API: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }


    
}