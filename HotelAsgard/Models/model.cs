using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
    private readonly string _baseUrl = "http://tu_api_url/api/users";

    public UserApiClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task<HttpResponseMessage> RegisterUserAsync(UserModel user)
    {
        var json = JsonSerializer.Serialize(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync($"{_baseUrl}/register", content);
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var json = JsonSerializer.Serialize(new { email, password });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/login", content);

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseData);
            return doc.RootElement.GetProperty("data").GetProperty("token").GetString();
        }
        return null;
    }

    public async Task<List<UserModel>?> GetAllUsersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<UserModel>>($"{_baseUrl}/getAll");
    }

    public async Task<UserModel?> GetUserByEmailAsync(string email)
    {
        var json = JsonSerializer.Serialize(new { email });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/getOne", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserModel>();
        }
        return null;
    }

    public async Task<HttpResponseMessage> UpdateUserAsync(UserModel user)
    {
        var json = JsonSerializer.Serialize(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PatchAsync($"{_baseUrl}/update", content);
    }

    public async Task<HttpResponseMessage> DeleteUserAsync(string email)
    {
        var json = JsonSerializer.Serialize(new { email });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync($"{_baseUrl}/delete", content);
    }
}
