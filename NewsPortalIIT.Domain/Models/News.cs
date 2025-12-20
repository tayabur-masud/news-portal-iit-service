using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NewsPortalIIT.Domain.Models;

public class News
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public ObjectId AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User Author { get; set; }
    public ICollection<Comment> Comments { get; set; }
}
