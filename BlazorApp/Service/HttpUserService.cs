using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Service;

public class HttpUserService : IUserService
{
    private readonly HttpClient _httpClient;

    public HttpUserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("users", request);
        response.EnsureSuccessStatusCode();

        var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();
        return createdUser!;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<UserDto>>("users");
        return response ?? new List<UserDto>();

    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<UserDto>($"users/{id}");
        return response;
    }

    public async Task UpdateUserAsync(int id, CreateUserDto request)
    {
        var httpResponse = await _httpClient.PutAsJsonAsync($"users/{id}", request);
        httpResponse.EnsureSuccessStatusCode(); 
    }

    public async Task DeleteUserAsync(int id)
    {
        var httpResponse = await _httpClient.DeleteAsync($"users/{id}");
        httpResponse.EnsureSuccessStatusCode();
    }
}