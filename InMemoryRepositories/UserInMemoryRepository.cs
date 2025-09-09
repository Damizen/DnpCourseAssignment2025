using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users = [];
    public UserInMemoryRepository()
    {
        SeedData();
    }

    private void SeedData()
    {
        users.AddRange([
            new User(1, "donald trump", "wall123"),
            new User(2, "sigma", "dupa"),
            new User(3, "beer", "iscalling")
        ]);
    }
    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any()
            ? users.Max(u => u.Id) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        var existingUser = GetUserById(user.Id);
        users.Remove(existingUser);
        users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var userToRemove = GetUserById(id);
        users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        var user = GetUserById(id);
        return Task.FromResult(user);
    }

    public IQueryable<User> GetManyAsync()
    {
        return users.AsQueryable();
    }
    
    private User GetUserById(int id)
    {
        var user = users.SingleOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }
        return user;
    }
}