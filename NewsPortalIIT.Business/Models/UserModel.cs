namespace NewsPortalIIT.Business.Models;

/// <summary>
/// Represents a user in the business layer.
/// Used for data transfer between the business layer and API layer.
/// </summary>
public class UserModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; }
}
