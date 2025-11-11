using ApiContracts;

namespace BlazorApp.Service;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserByIdAsync(int id);
    Task UpdateUserAsync(int id, CreateUserDto request);
    Task DeleteUserAsync(int id);
}