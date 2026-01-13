using NewsPortalIIT.Business.Models;

namespace NewsPortalIIT.Business.Services;

/// <summary>
/// Defines the contract for user management operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves all users from the system.
    /// </summary>
    /// <returns>A collection of all user models.</returns>
    Task<IEnumerable<UserModel>> GetAllAsync();

    /// <summary>
    /// Retrieves a specific user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The user model if found.</returns>
    /// <exception cref="Exception">Thrown when the user is not found.</exception>
    Task<UserModel> GetByIdAsync(string id);

    /// <summary>
    /// Creates a new user in the system.
    /// </summary>
    /// <param name="user">The user model containing the user information to create.</param>
    Task CreateAsync(UserModel user);

    /// <summary>
    /// Updates an existing user's information.
    /// </summary>
    /// <param name="user">The user model containing the updated user information.</param>
    /// <exception cref="Exception">Thrown when the user is not found.</exception>
    Task UpdateAsync(UserModel user);

    /// <summary>
    /// Deletes a user from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    Task DeleteAsync(string id);

    /// <summary>
    /// Authenticates a user based on their email and password.
    /// </summary>
    /// <param name="loginRequest">The login request containing credentials.</param>
    /// <returns>The authenticated user model if successful.</returns>
    Task<UserModel> LoginAsync(LoginRequest loginRequest);
}