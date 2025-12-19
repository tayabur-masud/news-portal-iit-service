using NewsPortalIIT.Business.Models;

namespace NewsPortalIIT.Business.Services;

public interface IUserService
{
    Task<IEnumerable<UserModel>> GetAllAsync();
    Task<UserModel> GetByIdAsync(string id);
    Task CreateAsync(UserModel user);
    Task UpdateAsync(UserModel user);
    Task DeleteAsync(string id);
}