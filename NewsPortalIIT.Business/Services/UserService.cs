using Mapster;
using MongoDB.Bson;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;
using NewsPortalIIT.Domain.Repositories;

namespace NewsPortalIIT.Business.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Adapt<IEnumerable<UserModel>>();
    }

    public async Task<UserModel> GetByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(ObjectId.Parse(id));
        return user.Adapt<UserModel>();
    }

    public async Task CreateAsync(UserModel userModel)
    {
        var user = userModel.Adapt<User>();
        await _userRepository.AddAsync(user);
    }

    public async Task UpdateAsync(UserModel userModel)
    {
        var user = userModel.Adapt<User>();
        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteAsync(string id)
    {
        await _userRepository.DeleteAsync(ObjectId.Parse(id));
    }
}
