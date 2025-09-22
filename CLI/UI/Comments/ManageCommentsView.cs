using Entities;
using RepositoryContracts;
namespace CLI.UI.Comments;

public class ManageCommentsView
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public ManageCommentsView(ICommentRepository commentRepository, IPostRepository postRepository, IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    public async Task ShowMenuAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("------POSTS MENU------");
            Console.WriteLine("1. Add comment");
            Console.WriteLine("2. See  comments");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");
            switch (Console.ReadLine())
            {
                case "1":
                    await AddComment();
                    break;
                case "2":
                    await ListComments();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }

    private async Task AddComment()
    {
        Console.Clear();
        Console.Write("Enter user ID: ");
        var userId = int.Parse(Console.ReadLine()!);
        Console.Write("Enter post ID: ");
        var postId = int.Parse(Console.ReadLine()!);
        Console.Write("Comment: ");
        var body = Console.ReadLine()!; 
        
        var comment = await _commentRepository.AddAsync(new Comment(0, userId, postId, body));
        Console.WriteLine($"Comment created with ID {comment.Id}");
        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

    private async Task ListComments()
    {
        Console.Clear();
        foreach (var p in _postRepository.GetManyAsync())
        {
            Console.WriteLine($"{p.Id} - {p.Title}");
        }
        Console.Write("Enter post ID: ");
        var postId = int.Parse(Console.ReadLine()!);
        Console.Clear();
        Console.WriteLine("-------Comments-------");
        foreach (var c in _commentRepository.GetManyAsync().Where(c => c.Post_Id == postId))
        {
            var user = await _userRepository.GetSingleAsync(c.User_Id);
            Console.WriteLine("----------------------");
            Console.WriteLine("Author name: " + user.Username);
            Console.WriteLine("Content:\n " + c.Body);
            Console.WriteLine("----------------------");
        }
        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }
}