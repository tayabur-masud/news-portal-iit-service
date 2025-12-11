namespace NewsPortalIIT.API.Models;

public class News
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User Author { get; set; }
    public List<Comment> Comments { get; set; }
}
