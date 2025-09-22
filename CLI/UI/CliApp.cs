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
        //Probably you are thinking rn what does it do. I had an issue with terminal focus
        //After the first run, the typed number was going way higher than Select option text is. 
        //This might be a stupid solution, but I have no idea what could fix it.
        Console.WriteLine("Click on this window and press any key to start..."); 
        Console.ReadKey(true);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- MENU ---");
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