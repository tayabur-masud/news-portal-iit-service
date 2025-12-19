namespace NewsPortalIIT.API.Models;

public class NewsResponse
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string AuthorId { get; set; }
    public string AuthorName { get; set; }
    public int NoOfComments { get; set; }
    public DateTime CreatedAt { get; set; }
}
