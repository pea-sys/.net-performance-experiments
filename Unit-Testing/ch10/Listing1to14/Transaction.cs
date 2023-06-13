using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Dapper;
using Moq;
using Xunit;

namespace ch10.Listing1to14
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; private set; }
        public UserType Type { get; private set; }
        public bool IsEmailConfirmed { get; }
        public List<IDomainEvent> DomainEvents { get; }

        public User(int userId, string email, UserType type, bool isEmailConfirmed)
        {
            UserId = userId;
            Email = email;
            Type = type;
            IsEmailConfirmed = isEmailConfirmed;
            DomainEvents = new List<IDomainEvent>();
        }

        public string CanChangeEmail()
        {
            if (IsEmailConfirmed)
                return "Can't change email after it's confirmed";

            return null;
        }

        public void ChangeEmail(string newEmail, Company company)
        {
            Precondition.Requires(CanChangeEmail() == null);

            if (Email == newEmail)
                return;

            UserType newType = company.IsEmailCorporate(newEmail)
                ? UserType.Employee
                : UserType.Customer;

            if (Type != newType)
            {
                int delta = newType == UserType.Employee ? 1 : -1;
                company.ChangeNumberOfEmployees(delta);
                AddDomainEvent(new UserTypeChangedEvent(UserId, Type, newType));
            }

            Email = newEmail;
            Type = newType;
            AddDomainEvent(new EmailChangedEvent(UserId, newEmail));
        }

        private void AddDomainEvent(IDomainEvent domainEvent)
        {
            DomainEvents.Add(domainEvent);
        }
    }

    public class UserController
    {
        private readonly Transaction _transaction;
        private readonly UserRepository _userRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly EventDispatcher _eventDispatcher;

        public UserController(
            Transaction transaction,
            MessageBus messageBus,
            IDomainLogger domainLogger)
        {
            _transaction = transaction;
            _userRepository = new UserRepository(transaction);
            _companyRepository = new CompanyRepository(transaction);
            _eventDispatcher = new EventDispatcher(
                messageBus, domainLogger);
        }

        public string ChangeEmail(int userId, string newEmail)
        {
            object[] userData = _userRepository.GetUserById(userId);
            User user = UserFactory.Create(userData);

            string error = user.CanChangeEmail();
            if (error != null)
                return error;

            object[] companyData = _companyRepository.GetCompany();
            Company company = CompanyFactory.Create(companyData);

            user.ChangeEmail(newEmail, company);

            _companyRepository.SaveCompany(company);
            _userRepository.SaveUser(user);
            _eventDispatcher.Dispatch(user.DomainEvents);

            _transaction.Commit();
            return "OK";
        }
    }

    public class EventDispatcher
    {
        private readonly MessageBus _messageBus;
        private readonly IDomainLogger _domainLogger;

        public EventDispatcher(
            MessageBus messageBus,
            IDomainLogger domainLogger)
        {
            _domainLogger = domainLogger;
            _messageBus = messageBus;
        }

        public void Dispatch(List<IDomainEvent> events)
        {
            foreach (IDomainEvent ev in events)
            {
                Dispatch(ev);
            }
        }

        private void Dispatch(IDomainEvent ev)
        {
            switch (ev)
            {
                case EmailChangedEvent emailChangedEvent:
                    _messageBus.SendEmailChangedMessage(
                        emailChangedEvent.UserId,
                        emailChangedEvent.NewEmail);
                    break;

                case UserTypeChangedEvent userTypeChangedEvent:
                    _domainLogger.UserTypeHasChanged(
                        userTypeChangedEvent.UserId,
                        userTypeChangedEvent.OldType,
                        userTypeChangedEvent.NewType);
                    break;
            }
        }
    }

    public class UserFactory
    {
        public static User Create(object[] data)
        {
            Precondition.Requires(data.Length >= 3);

            int id = (int)data[0];
            string email = (string)data[1];
            UserType type = (UserType)data[2];
            bool isEmailConfirmed = (bool)data[3];

            return new User(id, email, type, isEmailConfirmed);
        }
    }

    public class CompanyFactory
    {
        public static Company Create(object[] data)
        {
            Precondition.Requires(data.Length >= 2);

            string domainName = (string)data[0];
            int numberOfEmployees = (int)data[1];

            return new Company(domainName, numberOfEmployees);
        }
    }

    public interface IDomainLogger
    {
        void UserTypeHasChanged(int userId, UserType oldType, UserType newType);
    }

    public class DomainLogger : IDomainLogger
    {
        private readonly ILogger _logger;

        public DomainLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void UserTypeHasChanged(
            int userId, UserType oldType, UserType newType)
        {
            _logger.Info(
                $"User {userId} changed type " +
                $"from {oldType} to {newType}");
        }
    }

    public interface ILogger
    {
        void Info(string s);
    }

    public class UserTypeChangedEvent : IDomainEvent
    {
        public int UserId { get; }
        public UserType OldType { get; }
        public UserType NewType { get; }

        public UserTypeChangedEvent(int userId, UserType oldType, UserType newType)
        {
            UserId = userId;
            OldType = oldType;
            NewType = newType;
        }

        protected bool Equals(UserTypeChangedEvent other)
        {
            return UserId == other.UserId && string.Equals(OldType, other.OldType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((EmailChangedEvent)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (UserId * 397) ^ OldType.GetHashCode();
            }
        }
    }

    public class EmailChangedEvent : IDomainEvent
    {
        public int UserId { get; }
        public string NewEmail { get; }

        public EmailChangedEvent(int userId, string newEmail)
        {
            UserId = userId;
            NewEmail = newEmail;
        }

        protected bool Equals(EmailChangedEvent other)
        {
            return UserId == other.UserId && string.Equals(NewEmail, other.NewEmail);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((EmailChangedEvent)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (UserId * 397) ^ (NewEmail != null ? NewEmail.GetHashCode() : 0);
            }
        }
    }

    public interface IDomainEvent
    {
    }

    public class Company
    {
        public string DomainName { get; }
        public int NumberOfEmployees { get; private set; }

        public Company(string domainName, int numberOfEmployees)
        {
            DomainName = domainName;
            NumberOfEmployees = numberOfEmployees;
        }

        public void ChangeNumberOfEmployees(int delta)
        {
            Precondition.Requires(NumberOfEmployees + delta >= 0);

            NumberOfEmployees += delta;
        }

        public bool IsEmailCorporate(string email)
        {
            string emailDomain = email.Split('@')[1];
            return emailDomain == DomainName;
        }
    }

    public enum UserType
    {
        Customer = 1,
        Employee = 2
    }

    public static class Precondition
    {
        public static void Requires(bool precondition, string message = null)
        {
            if (precondition == false)
                throw new Exception(message);
        }
    }

    

    public class UserRepository
    {
        private readonly Transaction _transaction;

        public UserRepository(Transaction transaction)
        {
            _transaction = transaction;
        }

        public object[] GetUserById(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_transaction.ConnectionString))
            {
                string query = "SELECT * FROM [dbo].[User] WHERE UserID = @UserID";
                dynamic data = connection.QuerySingle(query, new { UserID = userId });

                return new object[]
                {
                    data.UserID,
                    data.Email,
                    data.Type,
                    data.IsEmailConfirmed
                };
            }
        }

        public void SaveUser(User user)
        {
            using (var connection = new SqlConnection(_transaction.ConnectionString))
            {
                string updateQuery = @"
                    UPDATE [dbo].[User]
                    SET Email = @Email, Type = @Type,
                        IsEmailConfirmed = @IsEmailConfirmed
                    WHERE UserID = @UserID
                    SELECT @UserID";

                string insertQuery = @"
                    INSERT [dbo].[User] (Email, Type, IsEmailConfirmed)
                    VALUES (@Email, @Type, @IsEmailConfirmed)
                    SELECT CAST(SCOPE_IDENTITY() as int)";

                string query = user.UserId == 0
                    ? insertQuery
                    : updateQuery;
                int userId = connection.Query<int>(query, new
                {
                    user.Email,
                    user.UserId,
                    user.IsEmailConfirmed,
                    Type = (int)user.Type
                })
                    .Single();

                user.UserId = userId;
            }
        }
    }

    public class CompanyRepository
    {
        private readonly Transaction _transaction;

        public CompanyRepository(Transaction transaction)
        {
            _transaction = transaction;
        }

        public object[] GetCompany()
        {
            using (SqlConnection connection = new SqlConnection(_transaction.ConnectionString))
            {
                string query = "SELECT * FROM dbo.Company";
                dynamic data = connection.QuerySingle(query);

                return new object[]
                {
                    data.DomainName,
                    data.NumberOfEmployees
                };
            }
        }

        public void SaveCompany(Company company)
        {
            using (var connection = new SqlConnection(_transaction.ConnectionString))
            {
                string query = @"
                    UPDATE dbo.Company
                    SET DomainName = @DomainName,
                        NumberOfEmployees = @NumberOfEmployees";

                connection.Execute(query, new
                {
                    company.DomainName,
                    company.NumberOfEmployees
                });
            }
        }
    }

    public class Transaction : IDisposable
    {
        private readonly TransactionScope _transaction;
        public readonly string ConnectionString;

        public Transaction(string connectionString)
        {
            _transaction = new TransactionScope();
            ConnectionString = connectionString;
        }

        public void Commit()
        {
            _transaction.Complete();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }
    }

    public class MessageBus
    {
        private readonly IBus _bus;

        public MessageBus(IBus bus)
        {
            _bus = bus;
        }

        public void SendEmailChangedMessage(int userId, string newEmail)
        {
            _bus.Send("Type: USER EMAIL CHANGED; " +
                $"Id: {userId}; " +
                $"NewEmail: {newEmail}");
        }
    }

    public interface IBus
    {
        void Send(string message);
    }

    public class BusSpy : IBus
    {
        private List<string> _sentMessages = new List<string>();

        public void Send(string message)
        {
            _sentMessages.Add(message);
        }

        public BusSpy ShouldSendNumberOfMessages(int number)
        {
            Assert.Equal(number, _sentMessages.Count);
            return this;
        }

        public BusSpy WithEmailChangedMessage(int userId, string newEmail)
        {
            string message = "Type: USER EMAIL CHANGED; " +
                $"Id: {userId}; " +
                $"NewEmail: {newEmail}";
            Assert.Contains(_sentMessages, x => x == message);

            return this;
        }
    }
}
