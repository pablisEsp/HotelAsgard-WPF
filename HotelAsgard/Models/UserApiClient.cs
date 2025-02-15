using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

public class UserModel
{
    public string Nombre { get; set; }
    public string Apellido1 { get; set; }
    public string? Apellido2 { get; set; }
    public string Email { get; set; }
    public string DniPasaporte { get; set; }
    public string Movil { get; set; }
    public string Password { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    public string Tipo { get; set; } = "usuario";
    public List<object> Reservas { get; set; } = new List<object>();
    public string? Avatar { get; set; }
}

public class UserApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:3000/api/users";
    private string? _token; // Para almacenar el token JWT

    public UserApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Registra un usuario en la API de Node.js
    /// </summary>
    public async Task<bool> RegisterUserAsync(UserModel user)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/register", user);
            var responseContent = await response.Content.ReadAsStringAsync(); // Capturar respuesta JSON
            Console.WriteLine($"📥 Respuesta API: {response.StatusCode} - {responseContent}"); // Debug

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Error: {responseContent}", "Error API", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error en RegisterUserAsync: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Inicia sesión y almacena el token JWT.
    /// </summary>
    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var requestBody = new { email, password }; // Ver qué datos se envían
            var json = JsonSerializer.Serialize(requestBody);
            Console.WriteLine($"📤 Enviando JSON: {json}"); // Debug

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/logincorporate", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"📥 Respuesta API: {responseString}"); // Debug

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden) // Manejar error 403 (Acceso denegado)
            {
                Console.WriteLine("❌ Acceso denegado: No eres administrador.");
                MessageBox.Show("❌ Acceso denegado: Solo los administradores pueden iniciar sesión.",
                    "Error de autenticación", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (response.IsSuccessStatusCode)
            {
                using JsonDocument doc = JsonDocument.Parse(responseString);
                _token = doc.RootElement.GetProperty("data").GetProperty("token").GetString();

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error en LoginAsync: {ex.Message}");
            return false;
        }
    }


    /// <summary>
    /// Obtiene todos los usuarios.
    /// </summary>
    public async Task<List<UserModel>?> GetAllUsersAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<UserModel>>($"{_baseUrl}/getAll");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en GetAllUsersAsync: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Obtiene un usuario por su email.
    /// </summary>
    public async Task<UserModel?> GetUserByEmailAsync(string email)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/getOne", new { email });

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<UserModel>();

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en GetUserByEmailAsync: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Obtiene usuarios filtrados por nombre, email, DNI o móvil.
    /// </summary>
    public async Task<List<UserModel>?> GetFilteredUsersAsync(string? nombre, string? email, string? dniPasaporte, string? movil)
    {
        try
        {
            var filter = new
            {
                nombre,
                email,
                dniPasaporte,
                movil
            };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/getFilter", filter);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<UserModel>>();

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en GetFilteredUsersAsync: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Actualiza un usuario usando su DNI/Pasaporte como identificador.
    /// </summary>
    public async Task<bool> UpdateUserAsync(UserModel user)
    {
        try
        {
            var response = await _httpClient.PatchAsJsonAsync($"{_baseUrl}/update", user);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en UpdateUserAsync: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Elimina un usuario por email.
    /// </summary>
    public async Task<bool> DeleteUserAsync(string email)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/delete", new { email });
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en DeleteUserAsync: {ex.Message}");
            return false;
        }
    }
}
