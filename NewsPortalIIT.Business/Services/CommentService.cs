using Mapster;
using MongoDB.Bson;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;
using NewsPortalIIT.Domain.Repositories;

namespace NewsPortalIIT.Business.Services;

/// <summary>
/// Provides implementation for comment management operations.
/// Handles CRUD operations for comments with optimized batch loading of related author data.
/// </summary>
public class CommentService : ICommentService
{
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<User> _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentService"/> class.
    /// </summary>
    /// <param name="commentRepository">The repository for comment data access.</param>
    /// <param name="userRepository">The repository for user data access.</param>
    public CommentService(
        IRepository<Comment> commentRepository,
        IRepository<User> userRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This method uses batch loading to efficiently retrieve author information for all comments,
    /// minimizing database queries by loading all required authors in a single query.
    /// </remarks>
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

    /// <inheritdoc/>
    public async Task CreateAsync(CommentModel commentModel)
    {
        var comment = commentModel.Adapt<Comment>();
        comment.CreatedAt = DateTime.UtcNow;
        await _commentRepository.AddAsync(comment);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(CommentModel commentModel)
    {
        var commentFromDb = await _commentRepository.GetByIdAsync(ObjectId.Parse(commentModel.Id));

        if (commentFromDb is null)
        {
            throw new Exception("Comment not found");
        }

        commentModel.Adapt(commentFromDb);

        await _commentRepository.UpdateAsync(commentFromDb);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string id)
    {
        await _commentRepository.DeleteAsync(ObjectId.Parse(id));
    }
}
