using NewsPortalIIT.Business.Models;

namespace NewsPortalIIT.Business.Services;

public interface INewsService
{
    Task<NewsModel> GetByIdAsync(string id);
    Task CreateAsync(NewsModel user);
    Task UpdateAsync(NewsModel user);
    Task DeleteAsync(string id);
    Task<PagedResult<NewsModel>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
}
