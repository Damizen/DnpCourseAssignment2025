using Entities;
using RepositoryContracts;

namespace CLI.UI.Posts;

public class ManagePostsView
{
    private readonly IPostRepository _postRepo;
    private readonly ICommentRepository _commentRepo;
    private readonly IUserRepository _userRepo;

    public ManagePostsView(IPostRepository postRepo, ICommentRepository commentRepo,  IUserRepository userRepo)
    {
        _postRepo = postRepo;
        _commentRepo = commentRepo;
        _userRepo = userRepo;
    }

    public async Task ShowMenuAsync()
    {
        Console.Clear();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n------POSTS MENU------");
            Console.WriteLine("1. Create a new post");
            Console.WriteLine("2. View Posts Overview");
            Console.WriteLine("3. View Specific Post");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");
            switch (Console.ReadLine())
            {
                case "1":
                    await CreateNewPost();
                    break;
                case "2":
                    await ViewPostsOverview();
                    break;
                case "3":
                    await ViewSpecificPost();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }

    }

    private async Task CreateNewPost()
    {
        Console.Clear();
        Console.Write("Enter ID for a user: ");
        var userId = int.Parse(Console.ReadLine()!);
        Console.Write("Enter Title: ");
        var title = Console.ReadLine()!;
        Console.Write("Enter Body: ");
        var body = Console.ReadLine()!;

        var post = await _postRepo.AddAsync((new Post(0, userId, title, body)));
        Console.WriteLine($"Post created with ID {post.Id}");
        Console.Write("Press any key to continue...");
        Console.ReadKey();
        ShowMenuAsync();
    }

    private async Task ViewPostsOverview()
    {
        Console.Clear();
        foreach (var p in _postRepo.GetManyAsync())
        {
            Console.WriteLine($"{p.Id}: {p.Title}");
        }
        Console.Write("Press any key to continue...");
        Console.ReadKey();
        ShowMenuAsync();
    }

    private async Task ViewSpecificPost()
    {
        Console.Clear();
        Console.Write("Enter post ID: ");
        
        var postId = int.Parse(Console.ReadLine()!);
        var post = await _postRepo.GetSingleAsync(postId);
        
        var comments = _commentRepo.GetManyAsync().Where(c => c.Post_Id == postId);
        Console.Clear();
        Console.WriteLine("Post ID: " + post.Id);
        Console.WriteLine("Title: " + post.Title);
        Console.WriteLine("Body: " + post.Body);
        Console.WriteLine("--- Comments ---");
        foreach (var c in comments)
        {
            var user = await _userRepo.GetSingleAsync(c.User_Id);
            Console.WriteLine($"{user.Username}: {c.Body}");
        }
        Console.Write("Press any key to continue...");
        Console.ReadKey();
        ShowMenuAsync();
    }
}