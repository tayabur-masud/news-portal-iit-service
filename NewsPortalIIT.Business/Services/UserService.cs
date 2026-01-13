using Mapster;
using MongoDB.Bson;
using NewsPortalIIT.Business.Models;
using NewsPortalIIT.Domain.Models;
using NewsPortalIIT.Domain.Repositories;
using BCryptNet = BCrypt.Net.BCrypt;

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
        user.PasswordHash = BCryptNet.HashPassword(userModel.Password);
        await _userRepository.AddAsync(user);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(UserModel userModel)
    {
        var userFromDb = await _userRepository.GetByIdAsync(ObjectId.Parse(userModel.Id));
        if (userFromDb is null)
        {
            throw new Exception("User not found");
        }

        userFromDb.PasswordHash = BCryptNet.HashPassword(userModel.Password);
        userModel.Adapt(userFromDb);

        await _userRepository.UpdateAsync(userFromDb);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string id)
    {
        await _userRepository.DeleteAsync(ObjectId.Parse(id));
    }

    /// <inheritdoc/>
    public async Task<UserModel> LoginAsync(LoginRequest loginRequest)
    {
        var users = await _userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Email == loginRequest.Email);

        if (user == null || !BCryptNet.Verify(loginRequest.Password, user.PasswordHash))
        {
            throw new Exception("Invalid email or password");
        }

        return user.Adapt<UserModel>();
    }
}
