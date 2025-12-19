using NewsPortalIIT.Business.Models;

namespace NewsPortalIIT.Business.Services;

public interface INewsService
{
    Task<IEnumerable<NewsModel>> GetAllAsync();
    Task<NewsModel> GetByIdAsync(string id);
    Task CreateAsync(NewsModel user);
    Task UpdateAsync(NewsModel user);
    Task DeleteAsync(string id);
}
