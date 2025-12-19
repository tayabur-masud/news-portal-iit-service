using NewsPortalIIT.Business.Models;

namespace NewsPortalIIT.Business.Services;

public interface ICommentService
{
    Task<IEnumerable<CommentModel>> GetByNewsIdAsync(string id);
    Task CreateAsync(CommentModel comment);
    Task UpdateAsync(CommentModel comment);
    Task DeleteAsync(string id);
}
