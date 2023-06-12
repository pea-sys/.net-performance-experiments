using Moq;

namespace ch9.V2
{
    public class IntegrationTests
    {
        private const string ConnectionString = @"Server=.\Sql;Database=IntegrationTests;Trusted_Connection=true;";

        [Fact]
        public void Changing_email_from_corporate_to_non_corporate()
        {
            // Arrange
            var db = new Database(ConnectionString);
            User user = CreateUser("user@mycorp.com", UserType.Employee, db);
            CreateCompany("mycorp.com", 1, db);

            var busMock = new Mock<IBus>();
            var messageBus = new MessageBus(busMock.Object);
            var loggerMock = new Mock<IDomainLogger>();
            var sut = new UserController(db, messageBus, loggerMock.Object);

            // Act
            string result = sut.ChangeEmail(user.UserId, "new@gmail.com");

            // Assert
            Assert.Equal("OK", result);

            object[] userData = db.GetUserById(user.UserId);
            User userFromDb = UserFactory.Create(userData);
            Assert.Equal("new@gmail.com", userFromDb.Email);
            Assert.Equal(UserType.Customer, userFromDb.Type);

            object[] companyData = db.GetCompany();
            Company companyFromDb = CompanyFactory.Create(companyData);
            Assert.Equal(0, companyFromDb.NumberOfEmployees);

            busMock.Verify(
                x => x.Send(
                    "Type: USER EMAIL CHANGED; " +
                    $"Id: {user.UserId}; " +
                    "NewEmail: new@gmail.com"),
                Times.Once);
            loggerMock.Verify(
                x => x.UserTypeHasChanged(
                    user.UserId, UserType.Employee, UserType.Customer),
                Times.Once);
        }

        [Fact]
        public void Changing_email_from_corporate_to_non_corporate_spy()
        {
            // Arrange
            var db = new Database(ConnectionString);
            User user = CreateUser("user@mycorp.com", UserType.Employee, db);
            CreateCompany("mycorp.com", 1, db);

            var busSpy = new BusSpy();
            var messageBus = new MessageBus(busSpy);
            var loggerMock = new Mock<IDomainLogger>();
            var sut = new UserController(db, messageBus, loggerMock.Object);

            // Act
            string result = sut.ChangeEmail(user.UserId, "new@gmail.com");

            // Assert
            Assert.Equal("OK", result);

            object[] userData = db.GetUserById(user.UserId);
            User userFromDb = UserFactory.Create(userData);
            Assert.Equal("new@gmail.com", userFromDb.Email);
            Assert.Equal(UserType.Customer, userFromDb.Type);

            object[] companyData = db.GetCompany();
            Company companyFromDb = CompanyFactory.Create(companyData);
            Assert.Equal(0, companyFromDb.NumberOfEmployees);

            busSpy.ShouldSendNumberOfMessages(1)
                .WithEmailChangedMessage(user.UserId, "new@gmail.com");
            loggerMock.Verify(
                x => x.UserTypeHasChanged(
                    user.UserId, UserType.Employee, UserType.Customer),
                Times.Once);
        }

        private Company CreateCompany(string domainName, int numberOfEmployees, Database database)
        {
            var company = new Company(domainName, numberOfEmployees);
            database.SaveCompany(company);
            return company;
        }

        private User CreateUser(string email, UserType type, Database database)
        {
            var user = new User(0, email, type, false);
            database.SaveUser(user);
            return user;
        }
    }

}
