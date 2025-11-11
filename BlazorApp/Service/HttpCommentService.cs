using System.Text.Json;
using ApiContracts;
using ApiContracts.Comments;

namespace BlazorApp.Service;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient _httpClient;

    public HttpCommentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync("comments", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<IEnumerable<CommentDto>> GetAllCommentsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<CommentDto>>("comments");
        return response ?? new List<CommentDto>();
    }

    public async Task<CommentDto> GetCommentByIdAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<CommentDto>($"comments/{id}");
        return response!;
    }

    public async Task UpdateCommentAsync(int id, CreateCommentDto request)
    {
        var httpResponse = await _httpClient.PutAsJsonAsync($"comments/{id}", request);
        httpResponse.EnsureSuccessStatusCode();
    }

    public async Task DeleteCommentAsync(int id)
    {
        var httpResponse = await _httpClient.DeleteAsync($"comments/{id}");
        httpResponse.EnsureSuccessStatusCode();
    }
}