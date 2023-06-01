
namespace ch6.Listing4to6
{
    public class Article
    {
        private readonly List<Comment> _comments = new List<Comment>();

        public IReadOnlyList<Comment> Comments =>
            _comments.ToList();

        public void AddComment(string text, string author, DateTime now)
        {
            _comments.Add(new Comment(text, author, now));
        }

        public Article ShouldContainNumberOfComments(int i)
        {
            return this;
        }
    }
}
