using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            var initialUsers = new List<User>
            {
                new User(1, "admin", "admin"),
                new User(2, "Michael", "dupa"),
                new User(3, "Greg", "password")
            };
            string json = JsonSerializer.Serialize(initialUsers);
            File.WriteAllText(filePath, json);
        }
    }

    public async Task<User> AddAsync(User user)
    {
        var users = await DeserializeUsers();
        int maxId = users.Count > 0 ? users.Max(c => c.Id) : 1;
        user.Id = maxId + 1;
        users.Add(user);
        SerializeUsers(users);
        return await Task.FromResult(user);
    }

    public async Task UpdateAsync(User user)
    {
        var users = await DeserializeUsers();
        var existingUser = GetUserById(user.Id, users);
        users.Remove(existingUser);
        users.Add(user);
        SerializeUsers(users);
    }

    public async Task DeleteAsync(int id)
    {
        var users  = await DeserializeUsers();
        var userToRemove = GetUserById(id, users);
        users.Remove(userToRemove);
        SerializeUsers(users);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        var users = await DeserializeUsers();
        var user = GetUserById(id, users);
        return await Task.FromResult(user);
    }

    public IQueryable<User> GetManyAsync()
    {
        string usersAsJson = File.ReadAllTextAsync(filePath).Result;
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users.AsQueryable();
    }
    private User GetUserById(int id, List<User> users)
    {
        var user = users.SingleOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }
        return user;
    }

    private async Task<List<User>> DeserializeUsers()
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        var users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users;
    }

    private async void SerializeUsers(List<User> users)
    {
        string usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, usersAsJson);
    }
    
}