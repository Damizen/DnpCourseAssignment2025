using ApiContracts.Posts;

namespace BlazorApp.Service;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<IEnumerable<PostDto>> GetAllPostsAsync();
    Task<PostDto> GetPostByIdAsync(int id);
    Task UpdatePostAsync(int id, CreatePostDto request);
    Task DeletePostAsync(int id);
}