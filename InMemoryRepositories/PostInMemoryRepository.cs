using Entities;
using RepositoryContracts;
namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts = [];
    
    public PostInMemoryRepository()
    {
        SeedData();
    }

    private void SeedData()
    {
        posts.AddRange(new[]
        {
            new Post(1, 1, "Sraka", "This is my last post goodbye"),
            new Post(2, 2, "why are we still here?", "Just to suffer"),
            new Post(3, 1, "Funny picture", "Hhhahha"),
            new Post(4, 3, "C# for dummies", "Console.WriteLine('Hello World');")
        });
    }
    
    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any() 
            ? posts.Max(p => p.Id) + 1 
            : 1; 
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        var existingPost = GetPostById(post.Id);
        posts.Remove(existingPost);
        posts.Add(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var postToRemove = GetPostById(id);
        posts.Remove(postToRemove); 
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        var post = GetPostById(id);
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetManyAsync()
    {
        return posts.AsQueryable();
    }
    
    private Post GetPostById(int id)
    {
        var post = posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }
        return post;
    }
}