using ApiContracts;
using ApiContracts.Comments;
using Entities;
using RepositoryContracts;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
    {
        try
        {
            var comment = new Comment(0, request.User_Id, request.Post_Id, request.Body);
            var created = await _commentRepository.AddAsync(comment);

            return Created($"/comments/{created.Id}", new CommentDto
            {
                Id = created.Id,
                User_Id = created.User_Id,
                Post_Id = created.Post_Id,
                Body = request.Body
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    public ActionResult<List<CommentDto>> GetAllComments([FromQuery] int? postId)
    {
        try
        {

            var comments = _commentRepository.GetManyAsync();
            List<CommentDto> dtoComments = new();
            
            if (postId != null)
            {
                comments = comments.Where(c => c.Post_Id == postId);
            }

            foreach (var comment in comments)
            {
                dtoComments.Add(new CommentDto()
                {
                    Id = comment.Id,
                    User_Id = comment.User_Id,
                    Post_Id = comment.Post_Id,
                    Body = comment.Body
                });
            }
            return Ok(dtoComments);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetComment([FromRoute] int id)
    {
        try
        {
            var comment = await _commentRepository.GetSingleAsync(id);

            return Ok(new CommentDto()
            {
                Id = comment.Id,
                User_Id = comment.User_Id,
                Post_Id = comment.Post_Id,
                Body = comment.Body
            });
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Comment with ID {id} not found.");  
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateComment(int id, [FromBody] CreateCommentDto request)
    {
        try
        {
            var comment = await _commentRepository.GetSingleAsync(id);
            comment.Body = request.Body;
            await _commentRepository.UpdateAsync(comment);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Comment with ID {id} not found.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteComment([FromRoute] int id)
    {
        try
        {
            await _commentRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Comment with ID {id} not found.");
        }
    }
}