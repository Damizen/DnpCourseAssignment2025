using ApiContracts.Comments;

namespace BlazorApp.Service;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    Task<IEnumerable<CommentDto>> GetAllCommentsAsync();
    Task<CommentDto> GetCommentByIdAsync(int id);
    Task UpdateCommentAsync(int id, CreateCommentDto request);
    Task DeleteCommentAsync(int id);
}