namespace NewsPortalIIT.API.Models;

/// <summary>
/// Represents the response returned after a successful authentication.
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Gets or sets the user information.
    /// </summary>
    public UserResponse User { get; set; }

    /// <summary>
    /// Gets or sets the JWT access token.
    /// </summary>
    public string Token { get; set; }
}
