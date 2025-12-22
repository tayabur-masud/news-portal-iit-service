namespace NewsPortalIIT.API.Models;

public class CommentRequest
{
    public string Text { get; set; }
    public string AuthorId { get; set; }
    public string NewsId { get; set; }
}
