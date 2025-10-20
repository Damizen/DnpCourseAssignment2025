using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    
    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        try
        {
            if (!await VerifyUserNameIsAvailableAsync(request.Username))
            {
                return Conflict($"Username '{request.Username}' is already taken.");
            }
            
            User user = new (0, request.Username, request.Password);
            User created = await _userRepository.AddAsync(user);
            return Created($"/users/{created.Id}", new UserDto 
            { 
                Id = created.Id, 
                Username = created.Username 
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetAllUsers([FromQuery] string? username)
    {
        var users = _userRepository.GetManyAsync();
        List<UserDto> dtoUsers = new();
        if (!string.IsNullOrWhiteSpace(username))
        {
            users = users.Where(u => u.Username.Contains(username, StringComparison.OrdinalIgnoreCase));
        }

        foreach(var user in users)
        {
            dtoUsers.Add(new UserDto
            {
                Id = user.Id,
                Username = user.Username
            });
        }

        return Ok(dtoUsers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        try
        {
            var user = await _userRepository.GetSingleAsync(id);
            return Ok(new UserDto{Id = user.Id, Username = user.Username});
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser(int id, [FromBody] CreateUserDto request)
    {
        try
        {
            var existing = await _userRepository.GetSingleAsync(id);
            existing.Username = request.Username;
            existing.Password = request.Password;
            await _userRepository.UpdateAsync(existing);
            return NoContent();
        }
        catch (Exception e)
        { 
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            await _userRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private async Task<bool> VerifyUserNameIsAvailableAsync(string username)
    {
        var users = _userRepository.GetManyAsync();

        bool exists = users.Any(u => 
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        
        return !exists;
    }
    
}