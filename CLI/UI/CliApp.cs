using RepositoryContracts;
using CLI.UI.Posts;
using CLI.UI.Comments;
using CLI.UI.Users;


namespace UI;

public class CliApp
{
    private readonly ManagerUsersView _userView;
    private readonly ManagePostsView _postView;
    private readonly ManageCommentsView _commentsView;

    public CliApp(IUserRepository userRepo, IPostRepository postRepo, ICommentRepository commentRepo)
    {
        _userView = new ManagerUsersView(userRepo);
        _postView = new ManagePostsView(postRepo, commentRepo, userRepo);
        _commentsView = new ManageCommentsView(commentRepo, postRepo, userRepo);
    }

    public async Task StartAsync()
    {
        Console.Clear();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n--- MENU ---");
            Console.WriteLine("1. Manage Users");
            Console.WriteLine("2. Manage Posts");
            Console.WriteLine("3. Manage Comments");
            Console.WriteLine("0. Exit");
            Console.Write("Select option: ");
            
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    await _userView.ShowMenuAsync();
                    break;
                case "2":
                    await _postView.ShowMenuAsync();
                    break;
                case "3":
                    await _commentsView.ShowMenuAsync();
                    break;
                case "0":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }
        }
    }
}