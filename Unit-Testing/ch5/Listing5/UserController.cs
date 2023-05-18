
namespace ch5.Listing5
{
    /// <summary>
    /// 実装の詳細が漏れているダメな設計例
    /// </summary>
    public class UserController
    {
        public void RenameUser(int userId, string newName)
        {
            User user = GetUserFromDatabase(userId);

            string normalizedName = user.NormalizeName(newName);
            user.Name = normalizedName;

            SaveUserToDatabase(user);
        }

        private void SaveUserToDatabase(User user)
        {
        }

        private User GetUserFromDatabase(int userId)
        {
            return new User();
        }
    }
}
