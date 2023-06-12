
namespace ch9.V2
{
    public class UserController
    {
        private readonly Database _database;
        private readonly EventDispatcher _eventDispatcher;

        public UserController(
            Database database,
            MessageBus messageBus,
            IDomainLogger domainLogger)
        {
            _database = database;
            _eventDispatcher = new EventDispatcher(
                messageBus, domainLogger);
        }

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
            _eventDispatcher.Dispatch(user.DomainEvents);

            return "OK";
        }
    }
}
