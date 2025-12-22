using Mapster;
using Microsoft.AspNetCore.Mvc;
using NewsPortalIIT.API.Models;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Business.Services;

namespace NewsPortalIIT.API.Controllers;

[Route("api/comments")]
[Consumes("application/json")]
[ApiController]
public class CommentsController(ICommentService commentService) : ControllerBase
{
    private readonly ICommentService _commentService = commentService;

    [HttpGet("news/{id}")]
    public async Task<IEnumerable<CommentResponse>> GetByNews(string id)
    {
        var comments = await _commentService.GetByNewsIdAsync(id);
        return comments.Adapt<IEnumerable<CommentResponse>>();
    }

    [HttpPost]
    public async Task Post([FromBody] CommentRequest model)
    {
        await _commentService.CreateAsync(model.Adapt<CommentModel>());
    }

    [HttpPut("{id}")]
    public async Task Put(string id, [FromBody] CommentRequest model)
    {
        var commentModel = model.Adapt<CommentModel>();
        commentModel.Id = id;
        await _commentService.UpdateAsync(commentModel);
    }

    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        await _commentService.DeleteAsync(id);
    }
}
