using MongoDB.Bson;

namespace NewsPortalIIT.Domain.Models;

public class User
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
