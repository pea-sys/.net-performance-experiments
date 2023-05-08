
namespace ch3.Listing1
{
    /// <summary>
    /// AAAパターン例示
    /// </summary>
    public class CalculatorTests
    {
        [Fact]
        public void Sum_of_two_numbers()
        {
            // Arrange
            double first = 10;
            double second = 20;
            var calculator = new Calculator();

            // Act
            double result = calculator.Sum(first, second);

            // Assert
            Assert.Equal(30, result);
        }
    }
}
