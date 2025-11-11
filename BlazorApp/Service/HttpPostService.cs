using System.Text.Json;
using ApiContracts.Posts;

namespace BlazorApp.Service;

public class HttpPostService : IPostService
{
    private readonly HttpClient _httpClient;

    public HttpPostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync("posts", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<PostDto>>("posts");
        return response ?? new List<PostDto>();
    }

    public async Task<PostDto> GetPostByIdAsync(int id)
    {
        var response =  await _httpClient.GetFromJsonAsync<PostDto>($"posts/{id}");
        return response!;
    }

    public async Task UpdatePostAsync(int id, CreatePostDto request)
    {
        var httpResponse = await _httpClient.PutAsJsonAsync($"posts/{id}", request);
        httpResponse.EnsureSuccessStatusCode();
    }

    public async Task DeletePostAsync(int id)
    {
        var httpResponse =  await _httpClient.DeleteAsync($"posts/{id}");
        httpResponse.EnsureSuccessStatusCode(); }
}