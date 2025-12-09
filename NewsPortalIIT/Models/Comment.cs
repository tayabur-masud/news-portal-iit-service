namespace NewsPortalIIT.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int AuthorId { get; set; }
        public int NewsId { get; set; }
        public DateTime CreatedAt { get; set; }

        public User Author { get; set; }
        public News News { get; set; }
    }
}
