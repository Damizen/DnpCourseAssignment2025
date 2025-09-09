using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private List<Comment> comments = [];
    
    public CommentInMemoryRepository()
    {
        SeedData();
    }

    private void SeedData()
    {
        comments.AddRange(new[]
        {
            new Comment(1, 2, 1, "You should quit"),    
            new Comment(2, 3, 1, "We won't miss you"),
            new Comment(3, 1, 2, "WTF is that"),
            new Comment(4, 2, 3, "Not funny didn't laugh"), 
            new Comment(5, 3, 3, "Bruh"), 
            new Comment(6, 1, 4, "Banana nano banana") 
        });
    }
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any()
            ? comments.Max(c => c.Id) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        var existingComment = GetCommentById(comment.Id);
        comments.Remove(existingComment);
        comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var commentToRemove = GetCommentById(id);
        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        var comment = GetCommentById(id);
        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return comments.AsQueryable();
    }
    
    private Comment GetCommentById(int id)
    {
        var comment = comments.SingleOrDefault(c => c.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }
        return comment;
    }
}