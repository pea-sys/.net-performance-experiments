
namespace ch7.Listing4
{
    public class Tests
    {
        [Fact]
        public void Changing_email_without_changing_user_type()
        {
            var company = new Company("mycorp.com", 1);
            var sut = new User(1, "user@mycorp.com", UserType.Employee);

            sut.ChangeEmail("new@mycorp.com", company);

            Assert.Equal(1, company.NumberOfEmployees);
            Assert.Equal("new@mycorp.com", sut.Email);
            Assert.Equal(UserType.Employee, sut.Type);
        }

        [Fact]
        public void Changing_email_from_corporate_to_non_corporate()
        {
            var company = new Company("mycorp.com", 1);
            var sut = new User(1, "user@mycorp.com", UserType.Employee);

            sut.ChangeEmail("new@gmail.com", company);

            Assert.Equal(0, company.NumberOfEmployees);
            Assert.Equal("new@gmail.com", sut.Email);
            Assert.Equal(UserType.Customer, sut.Type);
        }

        [Fact]
        public void Changing_email_from_non_corporate_to_corporate()
        {
            var company = new Company("mycorp.com", 1);
            var sut = new User(1, "user@gmail.com", UserType.Customer);

            sut.ChangeEmail("new@mycorp.com", company);

            Assert.Equal(2, company.NumberOfEmployees);
            Assert.Equal("new@mycorp.com", sut.Email);
            Assert.Equal(UserType.Employee, sut.Type);
        }

        [Fact]
        public void Changing_email_to_the_same_one()
        {
            var company = new Company("mycorp.com", 1);
            var sut = new User(1, "user@gmail.com", UserType.Customer);

            sut.ChangeEmail("user@gmail.com", company);

            Assert.Equal(1, company.NumberOfEmployees);
            Assert.Equal("user@gmail.com", sut.Email);
            Assert.Equal(UserType.Customer, sut.Type);
        }

        [InlineData("mycorp.com", "email@mycorp.com", true)]
        [InlineData("mycorp.com", "email@gmail.com", false)]
        [Theory]
        public void Differentiates_a_corporate_email_from_non_corporate(
            string domain, string email, bool expectedResult)
        {
            var sut = new Company(domain, 0);

            bool isEmailCorporate = sut.IsEmailCorporate(email);

            Assert.Equal(expectedResult, isEmailCorporate);
        }
    }
}
