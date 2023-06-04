namespace ch7.Listing2
{
    public class User
    {
        public int UserId { get; private set; }
        public string Email { get; private set; }
        public UserType Type { get; private set; }

        public User(int userId, string email, UserType type)
        {
            UserId = userId;
            Email = email;
            Type = type;
        }

        public int ChangeEmail(string newEmail,
            string companyDomainName, int numberOfEmployees)
        {
            if (Email == newEmail)
                return numberOfEmployees;

            string emailDomain = newEmail.Split('@')[1];
            bool isEmailCorporate = emailDomain == companyDomainName;
            UserType newType = isEmailCorporate
                ? UserType.Employee
                : UserType.Customer;

            if (Type != newType)
            {
                int delta = newType == UserType.Employee ? 1 : -1;
                int newNumber = numberOfEmployees + delta;
                numberOfEmployees = newNumber;
            }

            Email = newEmail;
            Type = newType;

            return numberOfEmployees;
        }
    }
}
