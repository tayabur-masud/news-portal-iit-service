using NewsPortalIIT.Business.Models;

namespace NewsPortalIIT.API.Services;

/// <summary>
/// Defines the contract for JWT token generation service.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user model to generate the token for.</param>
    /// <returns>A signed JWT token string.</returns>
    string GenerateToken(UserModel user);
}
