using Entities;
using RepositoryContracts;
namespace CLI.UI.Users;

public class ManagerUsersView
{ 
    private readonly IUserRepository _userRepository;

    public ManagerUsersView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task ShowMenuAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n--- USER MENU ---");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. List Users");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");
        
            var choice = Console.ReadLine();
            switch (choice)
            {
                case  "1": 
                    await CreateUserAsync();
                    break;
                case "2":
                    await ListUsers();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }
    private async Task CreateUserAsync()
    {
        Console.Clear();
        Console.Write("Enter username: ");
        var username = Console.ReadLine()!;
        Console.Write("Enter password: ");
        var password = Console.ReadLine()!;
        
        var user = await _userRepository.AddAsync(new User(0,username, password));
        Console.WriteLine($"User {username} has been created");
        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

     async Task ListUsers()
    {
        Console.Clear();
        var Users = _userRepository.GetManyAsync();
        foreach (var u in Users)
        {
            Console.WriteLine($"[{u.Id}] {u.Username}");
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}


