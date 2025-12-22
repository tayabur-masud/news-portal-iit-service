using Mapster;
using MongoDB.Bson;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;
using NewsPortalIIT.Domain.Repositories;

namespace NewsPortalIIT.Business.Services;

public class CommentService : ICommentService
{
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<User> _userRepository;

    public CommentService(
        IRepository<Comment> commentRepository,
        IRepository<User> userRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<CommentModel>> GetByNewsIdAsync(string id)
    {
        var commentsList = await _commentRepository.GetAsync(x => x.NewsId == ObjectId.Parse(id));
        var commentModels = commentsList.Adapt<IEnumerable<CommentModel>>().ToList();

        foreach (var commentModel in commentModels)
        {
            if (ObjectId.TryParse(commentModel.AuthorId, out var authorId))
            {
                var author = await _userRepository.GetByIdAsync(authorId);
                commentModel.Author = author.Adapt<UserModel>();
            }
        }

        return commentModels;
    }

    public async Task CreateAsync(CommentModel commentModel)
    {
        var comment = commentModel.Adapt<Comment>();
        comment.CreatedAt = DateTime.UtcNow;
        await _commentRepository.AddAsync(comment);
    }

    public async Task UpdateAsync(CommentModel commentModel)
    {
        var commentFromDb = await _commentRepository.GetByIdAsync(ObjectId.Parse(commentModel.Id));

        if (commentFromDb is null)
        {
            throw new Exception("Comment not found");
        }

        var comment = commentModel.Adapt<Comment>();
        comment.CreatedAt = commentFromDb.CreatedAt;
        await _commentRepository.UpdateAsync(comment);
    }

    public async Task DeleteAsync(string id)
    {
        await _commentRepository.DeleteAsync(ObjectId.Parse(id));
    }
}
