using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

    public async Task<List<Category>> GetCategorias()
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
            MessageBox.Show("Error al obtener el nuevo código de habitación.", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
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

    public async Task<List<Room>> SearchRooms(string codigo, string nombre, string categoria, int? numPersonas,
        int? tamanyoMin, int? tamanyoMax, decimal? precioMin, decimal? precioMax, bool? habilitada)
    {
        try
        {
            var url = "http://localhost:3000/api/rooms/filter?";

            if (!string.IsNullOrEmpty(codigo)) url += $"codigo={codigo}&";
            if (!string.IsNullOrEmpty(nombre)) url += $"nombre={nombre}&";
            if (!string.IsNullOrEmpty(categoria)) url += $"categoria={categoria}&";
            if (numPersonas.HasValue) url += $"numPersonas={numPersonas.Value}&";
            if (tamanyoMin.HasValue) url += $"tamanyoMin={tamanyoMin}&";
            if (tamanyoMax.HasValue) url += $"tamanyoMax={tamanyoMax}&";
            if (precioMin.HasValue) url += $"precioMin={precioMin}&";
            if (precioMax.HasValue) url += $"precioMax={precioMax}&";
            if (habilitada.HasValue) url += $"habilitada={habilitada.ToString().ToLower()}&";


            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var habitaciones = JsonConvert.DeserializeObject<List<Room>>(json);

            return habitaciones;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al filtrar habitaciones: {ex.Message}");
        }
    }

    public async Task<bool> ActualizarHabitacionAsync(Room room, List<string> imagePaths)
    {
        try
        {
            string url = $"http://localhost:3000/api/rooms/{room.Codigo}";

            var formData = new MultipartFormDataContent();

            // Asegurar que se envían todas las imágenes actuales (incluyendo las que no son nuevas)
            formData.Add(new StringContent(JsonConvert.SerializeObject(imagePaths)), "imagenes");

            formData.Add(new StringContent(room.Codigo), "codigo");
            formData.Add(new StringContent(room.Nombre), "nombre");
            formData.Add(new StringContent(room.Categoria), "categoria");
            formData.Add(new StringContent(room.Tamanyo.ToString()), "tamanyo");
            formData.Add(new StringContent(room.NumPersonas.ToString()), "numPersonas");
            formData.Add(new StringContent(room.Precio.ToString()), "precio");
            formData.Add(new StringContent(room.Descripcion), "descripcion");
            formData.Add(new StringContent(room.Habilitada ? "true" : "false"), "habilitada");

            // Serializar `camas` y `servicios`
            formData.Add(new StringContent(JsonConvert.SerializeObject(room.Camas)), "camas");
            formData.Add(new StringContent(JsonConvert.SerializeObject(room.Servicios)), "servicios");

            // Agregar imágenes nuevas (archivos físicos)
            foreach (var imagePath in imagePaths)
            {
                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    formData.Add(imageContent, "imagenes", Path.GetFileName(imagePath));
                }
            }

            HttpResponseMessage response = await _httpClient.PutAsync(url, formData);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al actualizar la habitación: {ex.Message}", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return false;
        }
    }
    
    public async Task<bool> RoomExists(string codigo)
    {
        if (string.IsNullOrEmpty(codigo))
        {
            return false;
        }

        string url = $"http://localhost:3000/api/rooms/{codigo}";
        
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return true; // Si la API devuelve un 200, la habitación existe
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false; // Si la API devuelve un 404, la habitación no existe
            }

            return false; // Si hay otro error, asumimos que no existe
        }
        catch (Exception ex)
        {
            return false; // Si ocurre un error, asumimos que la habitación no existe
        }
    }
}