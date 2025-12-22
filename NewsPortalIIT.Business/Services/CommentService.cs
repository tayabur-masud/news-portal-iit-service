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
        var newsId = ObjectId.Parse(id);
        var commentsList = (await _commentRepository.GetAsync(x => x.NewsId == newsId)).ToList();
        var commentModels = commentsList.Adapt<IEnumerable<CommentModel>>().ToList();

        if (commentModels.Count == 0) return commentModels;

        // Batch load Authors
        var authorIds = commentsList
            .Where(c => c.AuthorId != ObjectId.Empty)
            .Select(c => c.AuthorId)
            .Distinct()
            .ToList();

        var authors = await _userRepository.GetAsync(u => authorIds.Contains(u.Id));
        var authorMap = authors.Adapt<IEnumerable<UserModel>>().ToDictionary(a => a.Id);

        // Assign Authors
        foreach (var commentModel in commentModels)
        {
            if (authorMap.TryGetValue(commentModel.AuthorId, out var author))
            {
                commentModel.Author = author;
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
