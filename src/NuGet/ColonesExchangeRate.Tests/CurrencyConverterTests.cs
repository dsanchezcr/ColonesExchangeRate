using Xunit;
namespace ColonesExchangeRate.Tests
{
    public class CurrencyConverterTests
    {
        private readonly CurrencyConverter _converter;
        public CurrencyConverterTests()
        {
            _converter = new CurrencyConverter();
        }
        [Fact]
        public async Task DollarsToColones_ShouldReturnNonNegativeDecimal()
        {
            // Arrange
            decimal amount = 10;

            // Act
            var result = await _converter.DollarsToColones(amount);

            // Assert
            Assert.True(result > 0);
        }
        [Fact]
        public async Task ColonesToDollars_ShouldReturnNonNegativeDecimal()
        {
            // Arrange
            decimal amount = 10000;

            // Act
            var result = await _converter.ColonesToDollars(amount);

            // Assert
            Assert.True(result > 0);
        }
        [Fact]
        public async Task DollarsToEuros_ShouldReturnNonNegativeDecimal()
        {
            // Arrange
            decimal amount = 10;

            // Act
            var result = await _converter.DollarsToEuros(amount);

            // Assert
            Assert.True(result > 0);
        }
        [Fact]
        public async Task EurosToDollars_ShouldReturnNonNegativeDecimal()
        {
            // Arrange
            decimal amount = 10;

            // Act
            var result = await _converter.EurosToDollars(amount);

            // Assert
            Assert.True(result > 0);
        }
        [Fact]
        public async Task EurosToColones_ShouldReturnNonNegativeDecimal()
        {
            // Arrange
            decimal amount = 10;

            // Act
            var result = await _converter.EurosToColones(amount);

            // Assert
            Assert.True(result > 0);
        }
        [Fact]
        public async Task ColonesToEuros_ShouldReturnNonNegativeDecimal()
        {
            // Arrange
            decimal amount = 10000;

            // Act
            var result = await _converter.ColonesToEuros(amount);

            // Assert
            Assert.True(result > 0);
        }
        [Fact]
        public async Task GetDollarExchangeRate_ShouldReturnNonNegativeValues()
        {
            // Act
            var (date, sale, purchase) = await _converter.GetDollarExchangeRate(true);

            // Assert
            Assert.True(sale > 0);
            Assert.True(purchase > 0);
            Assert.False(date == null);
        }
        [Fact]
        public async Task GetEuroExchangeRate_ShouldReturnNonNegativeValues()
        {
            // Act
            var (date, dollars, colones) = await _converter.GetEuroExchangeRate(true);

            // Assert
            Assert.True(dollars > 0);
            Assert.True(colones > 0);
            Assert.False(date == null);
        }
    }
}