using Mapster;
using MongoDB.Bson;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;
using NewsPortalIIT.Domain.Repositories;

namespace NewsPortalIIT.Business.Services;

public class CommentService : ICommentService
{
    private readonly IRepository<Comment> _commentRepository;

    public CommentService(IRepository<Comment> commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<IEnumerable<CommentModel>> GetByNewsIdAsync(string id)
    {
        var comments = await _commentRepository.GetAsync(x => x.NewsId == ObjectId.Parse(id), x => x.Author);
        return comments.Adapt<IEnumerable<CommentModel>>();
    }

    public async Task CreateAsync(CommentModel commentModel)
    {
        var comment = commentModel.Adapt<Comment>();
        await _commentRepository.AddAsync(comment);
    }

    public async Task UpdateAsync(CommentModel commentModel)
    {
        var comment = commentModel.Adapt<Comment>();
        await _commentRepository.UpdateAsync(comment);
    }

    public async Task DeleteAsync(string id)
    {
        await _commentRepository.DeleteAsync(ObjectId.Parse(id));
    }
}
