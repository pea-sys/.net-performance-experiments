
namespace ch6.Listing4to6
{
    public class CustomerControllerTests
    {
        /// <summary>
        /// 状態ベーステスト
        /// </summary>
        [Fact]
        public void Adding_a_comment_to_an_article()
        {
            var sut = new Article();
            var text = "Comment text";
            var author = "John Doe";
            var now = new DateTime(2019, 4, 1);

            sut.AddComment(text, author, now);

            Assert.Equal(1, sut.Comments.Count);
            Assert.Equal(text, sut.Comments[0].Text);
            Assert.Equal(author, sut.Comments[0].Author);
            Assert.Equal(now, sut.Comments[0].DateCreated);
        }
        /// <summary>
        /// 状態ベーステスト(ヘルパーメソッド利用)
        /// </summary>
        [Fact]
        public void Adding_a_comment_to_an_article2()
        {
            var sut = new Article();
            var text = "Comment text";
            var author = "John Doe";
            var now = new DateTime(2019, 4, 1);

            sut.AddComment(text, author, now);

            sut.ShouldContainNumberOfComments(1)
                .WithComment(text, author, now);
        }
        /// <summary>
        /// 状態ベーステスト(値比較チェック)
        /// </summary>
        [Fact]
        public void Adding_a_comment_to_an_article3()
        {
            var sut = new Article();
            var comment = new Comment(
                "Comment text",
                "John Doe",
                new DateTime(2019, 4, 1));

            sut.AddComment(comment.Text, comment.Author, comment.DateCreated);

            sut.Comments[0].Should()
                .BeEquivalentTo(comment);
        }
    }
}
