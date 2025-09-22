using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            var initialPosts = new List<Post>
            {
                new Post(1, 1, "Welcome Post", "This is the first post by admin."),
                new Post(2, 2, "Hello World", "This is Michael's first post."),
                new Post(3, 3, "Greetings", "Greg says hello!")
            };
            string json = JsonSerializer.Serialize(initialPosts);
            File.WriteAllText(filePath, json);
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        var posts = await DeserializePosts();
        int maxId = posts.Count > 0 ? posts.Max(c => c.Id) : 1;
        post.Id = maxId + 1;
        posts.Add(post);
        SerializePosts(posts);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        var posts = await DeserializePosts();
        var existingPost = GetPostById(post.Id, posts);
        posts.Remove(existingPost);
        posts.Add(post);
        SerializePosts(posts);
    }

    public async Task DeleteAsync(int id)
    {
        var posts  = await DeserializePosts();
        var postToRemove = GetPostById(id, posts);
        posts.Remove(postToRemove);
        SerializePosts(posts);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        var posts = await DeserializePosts();
        var post = GetPostById(id, posts);
        return await Task.FromResult(post);
    }

    public IQueryable<Post> GetManyAsync()
    {
        string postsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.AsQueryable();
    }
    
    private Post GetPostById(int id, List<Post> posts)
    {
        var post = posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }
        return post;
    }
    private async Task<List<Post>> DeserializePosts()
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        var posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts;
    }

    private async void SerializePosts(List<Post> posts)
    {
        string postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
    }
}