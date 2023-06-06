
namespace ch7.Listing14
{
    public class UserController
    {
        private readonly Database _database = new Database();
        private readonly MessageBus _messageBus = new MessageBus();

        public string ChangeEmail(int userId, string newEmail)
        {
            object[] userData = _database.GetUserById(userId);
            User user = UserFactory.Create(userData);

            string error = user.CanChangeEmail();
            if (error != null)
                return error;

            object[] companyData = _database.GetCompany();
            Company company = CompanyFactory.Create(companyData);

            user.ChangeEmail(newEmail, company);

            _database.SaveCompany(company);
            _database.SaveUser(user);
            foreach (EmailChangedEvent ev in user.EmailChangedEvents)
            {
                _messageBus.SendEmailChangedMessage(ev.UserId, ev.NewEmail);
            }

            return "OK";
        }
    }
}
