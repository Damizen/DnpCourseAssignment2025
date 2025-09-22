using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            var initialComments = new List<Comment>
            {
                new Comment(1, 1, 1, "Welcome to the platform!"),
                new Comment(2, 2, 1, "Thanks for the welcome!"),
                new Comment(3, 3, 2, "Nice post, Michael!"),
                new Comment(4, 1, 3, "Hello Greg!"),
                new Comment(5, 2, 3, "Hi Greg, good to see you here.")
            };
            string json = JsonSerializer.Serialize(initialComments);
            File.WriteAllText(filePath, json);
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        var comments = await DeserializeComments();
        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 1;
        comment.Id = maxId + 1;
        comments.Add(comment);
        SerializePosts(comments);
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        var comments = await DeserializeComments();
        var existingComment = GetCommentById(comment.Id, comments);
        comments.Remove(existingComment);
        comments.Add(comment);
        SerializePosts(comments);
    }

    public async Task DeleteAsync(int id)
    {
        var comments  = await DeserializeComments();
        var commentToRemove = GetCommentById(id, comments);
        comments.Remove(commentToRemove);
        SerializePosts(comments);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        var comments = await DeserializeComments();
        var comment = GetCommentById(id, comments);
        return await Task.FromResult(comment);
    }

    public IQueryable<Comment> GetManyAsync()
    {
        string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return comments.AsQueryable();
    }
    private Comment GetCommentById(int id, List<Comment> comments)
    {
        var comment = comments.SingleOrDefault(c => c.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }
        return comment;
    }
    private async Task<List<Comment>> DeserializeComments()
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        var comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return comments;
    }

    private async void SerializePosts(List<Comment> comments)
    {
        string commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }
}