namespace NewsPortalIIT.API.Models;

public class CommentResponse
{
    public string Id { get; set; }
    public string Text { get; set; }
    public string AuthorId { get; set; }
    public string AuthorName { get; set; }    
    public string NewsId { get; set; }
    public DateTime CreatedAt { get; set; }
}
