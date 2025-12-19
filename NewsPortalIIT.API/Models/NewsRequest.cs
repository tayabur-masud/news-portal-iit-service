namespace NewsPortalIIT.API.Models;

public class NewsRequest
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
}
