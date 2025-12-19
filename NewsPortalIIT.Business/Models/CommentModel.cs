namespace NewsPortalIIT.Business.Models;

public class CommentModel
{
    public string Id { get; set; }
    public string Text { get; set; }
    public string AuthorId { get; set; }
    public string NewsId { get; set; }
    public DateTime CreatedAt { get; set; }

    public UserModel Author { get; set; }
    public NewsModel News { get; set; }
}
