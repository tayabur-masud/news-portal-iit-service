using Mapster;
using MongoDB.Bson;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;
using NewsPortalIIT.Domain.Repositories;

namespace NewsPortalIIT.Business.Services;

/// <summary>
/// Provides implementation for user management operations.
/// Handles CRUD operations for users using the repository pattern and Mapster for object mapping.
/// </summary>
public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userRepository">The repository for user data access.</param>
    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Adapt<IEnumerable<UserModel>>();
    }

    /// <inheritdoc/>
    public async Task<UserModel> GetByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(ObjectId.Parse(id));
        if (user is null)
        {
            throw new Exception("User not found");
        }

        return user.Adapt<UserModel>();
    }

    /// <inheritdoc/>
    public async Task CreateAsync(UserModel userModel)
    {
        var user = userModel.Adapt<User>();
        await _userRepository.AddAsync(user);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(UserModel userModel)
    {
        var user = userModel.Adapt<User>();
        if (user is null)
        {
            throw new Exception("User not found");
        }

        await _userRepository.UpdateAsync(user);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string id)
    {
        await _userRepository.DeleteAsync(ObjectId.Parse(id));
    }
}
