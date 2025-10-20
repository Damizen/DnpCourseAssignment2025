using ApiContracts;
using ApiContracts.Posts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;
[ApiController]
[Route("[controller]")]

public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    
    public PostsController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
    {
        try
        {
            var post = new Post(0, request.UserId, request.Title, request.Body);
            var created = await _postRepository.AddAsync(post);
            
            return Created($"/posts/{created.Id}", new PostDto 
            { 
                Id = created.Id, 
                UserId = created.UserId,
                Title = created.Title,
                Body = created.Body
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    public ActionResult<List<PostDto>> GetAllPosts([FromQuery] string? title)
    {
        try
        {
            var posts = _postRepository.GetManyAsync();
            List<PostDto> dtoPosts = new();
        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        foreach (var post in posts)
        {
            dtoPosts.Add(new PostDto()
            {
                Id = post.Id,
                UserId = post.UserId,
                Title = post.Title,
                Body = post.Body
            });
        }
            return Ok(dtoPosts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetPost([FromRoute] int id)
    {
        try
        {
            var post = await _postRepository.GetSingleAsync(id);

            return Ok(new PostDto()
            {
                Id = post.Id,
                UserId = post.UserId,
                Title = post.Title,
                Body = post.Body
            });
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with ID {id} not found.");  
        } 
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdatePost(int id, [FromBody] CreatePostDto request)
    {
        try
        {
            var post =  await _postRepository.GetSingleAsync(id);
            post.UserId = request.UserId;
            post.Title = request.Title;
            post.Body = request.Body;
            await _postRepository.UpdateAsync(post);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with ID {id} not found.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePost([FromRoute] int id)
    {
        try
        {
            await _postRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with ID {id} not found.");
        }
    }
}