using Xunit;
public class CurrencyConverterTests
{
    private readonly ColonesExchangeRate _colonesExchangeRate;
    public CurrencyConverterTests()
    {
        _colonesExchangeRate = new ColonesExchangeRate();
    }
    [Fact]
    public async Task DollarsToColones_ShouldReturnNonNegativeDecimal()
    {
        // Arrange
        decimal amount = 10;

        // Act
        var result = await _colonesExchangeRate.DollarsToColones(amount);

        // Assert
        Assert.True(result > 0);
    }
    [Fact]
    public async Task ColonesToDollars_ShouldReturnNonNegativeDecimal()
    {
        // Arrange
        decimal amount = 10000;

        // Act
        var result = await _colonesExchangeRate.ColonesToDollars(amount);

        // Assert
        Assert.True(result > 0);
    }
    [Fact]
    public async Task DollarsToEuros_ShouldReturnNonNegativeDecimal()
    {
        // Arrange
        decimal amount = 10;

        // Act
        var result = await _colonesExchangeRate.DollarsToEuros(amount);

        // Assert
        Assert.True(result > 0);
    }
    [Fact]
    public async Task EurosToDollars_ShouldReturnNonNegativeDecimal()
    {
        // Arrange
        decimal amount = 10;

        // Act
        var result = await _colonesExchangeRate.EurosToDollars(amount);

        // Assert
        Assert.True(result > 0);
    }
    [Fact]
    public async Task EurosToColones_ShouldReturnNonNegativeDecimal()
    {
        // Arrange
        decimal amount = 10;

        // Act
        var result = await _colonesExchangeRate.EurosToColones(amount);

        // Assert
        Assert.True(result > 0);
    }
    [Fact]
    public async Task ColonesToEuros_ShouldReturnNonNegativeDecimal()
    {
        // Arrange
        decimal amount = 10000;

        // Act
        var result = await _colonesExchangeRate.ColonesToEuros(amount);

        // Assert
        Assert.True(result > 0);
    }
    [Fact]
    public async Task GetDollarExchangeRate_ShouldReturnValues()
    {
        // Act
        var (date, sale, purchase) = await _colonesExchangeRate.GetDollarExchangeRate();

        // Assert
        Assert.False(date == null);
        Assert.True(sale > 0);
        Assert.True(purchase > 0);           
    }
    [Fact]
    public async Task GetEuroExchangeRate_ShouldReturnValues()
    {
        // Act
        var (date, dollars, colones) = await _colonesExchangeRate.GetEuroExchangeRate();

        // Assert
        Assert.False(date == null);
        Assert.True(dollars > 0);
        Assert.True(colones > 0 || colones == null);            
    }
}