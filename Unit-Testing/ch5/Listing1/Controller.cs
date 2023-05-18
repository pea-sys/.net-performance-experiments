
namespace ch5.Listing1
{
    public class Controller
    {
        private readonly IEmailGateway _emailGateway;
        private readonly IDatabase _database;

        public Controller(IEmailGateway emailGateway)
        {
            _emailGateway = emailGateway;
        }

        public Controller(IDatabase database)
        {
            _database = database;
        }

        public void GreetUser(string userEmail)
        {
            _emailGateway.SendGreetingsEmail(userEmail);
        }

        public Report CreateReport()
        {
            int numberOfUsers = _database.GetNumberOfUsers();
            return new Report(numberOfUsers);
        }
    }
}
