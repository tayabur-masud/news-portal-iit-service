namespace NewsPortalIIT.Business.Models;

public class NewsModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }

    public UserModel Author { get; set; }
    public ICollection<CommentModel> Comments { get; set; }
}
