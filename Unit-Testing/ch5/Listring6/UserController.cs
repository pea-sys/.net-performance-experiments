
namespace ch5.Listring6
{
    public class UserController
    {
        public void RenameUser(int userId, string newName)
        {
            User user = GetUserFromDatabase(userId);
            user.Name = newName;
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
