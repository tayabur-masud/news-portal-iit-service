using MongoDB.Bson;

namespace NewsPortalIIT.Domain.Models;

public class Comment
{
    public ObjectId Id { get; set; }
    public string Text { get; set; }
    public ObjectId AuthorId { get; set; }
    public ObjectId NewsId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User Author { get; set; }
    public News News { get; set; }
}
