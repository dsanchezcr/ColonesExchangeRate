using System.Net;
using System.Text.Json;
using Xunit;

/// <summary>
/// A test HTTP handler that returns a predefined JSON response.
/// </summary>
public class MockHttpHandler : DelegatingHandler
{
    private readonly string _responseJson;

    public MockHttpHandler(string responseJson)
    {
        _responseJson = responseJson;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(_responseJson, System.Text.Encoding.UTF8, "application/json")
        });
    }
}

/// <summary>
/// Unit tests with mocked HTTP responses for deterministic validation.
/// </summary>
public class CurrencyConverterUnitTests
{
    // Sample API response with both dollar and euro data (business hours format)
    private const string FullApiResponse = @"{
        ""dolar"": {
            ""venta"": { ""fecha"": ""2025-01-15"", ""valor"": 515.50 },
            ""compra"": { ""fecha"": ""2025-01-15"", ""valor"": 505.25 }
        },
        ""euro"": {
            ""fecha"": ""2025-01-15"",
            ""dolares"": 1.08,
            ""colones"": 545.00,
            ""valor"": ""1.08""
        }
    }";

    // Sample API response without euro colones (after business hours format)
    private const string AfterHoursApiResponse = @"{
        ""dolar"": {
            ""venta"": { ""fecha"": ""2025-01-15"", ""valor"": 515.50 },
            ""compra"": { ""fecha"": ""2025-01-15"", ""valor"": 505.25 }
        },
        ""euro"": {
            ""fecha"": ""2025-01-15"",
            ""dolares"": 1.08,
            ""valor"": ""1.08""
        }
    }";

    private static ColonesExchangeRate CreateConverter(string responseJson)
    {
        var handler = new MockHttpHandler(responseJson);
        var httpClient = new HttpClient(handler);
        return new ColonesExchangeRate(httpClient, TimeSpan.Zero);
    }

    [Fact]
    public async Task DollarsToColones_ReturnsCorrectConversion()
    {
        var converter = CreateConverter(FullApiResponse);
        var result = await converter.DollarsToColones(10);
        Assert.Equal(5155.0m, result);
    }

    [Fact]
    public async Task ColonesToDollars_ReturnsCorrectConversion()
    {
        var converter = CreateConverter(FullApiResponse);
        var result = await converter.ColonesToDollars(5052.5m);
        Assert.Equal(10m, result, 10); // precision tolerance for decimal division
    }

    [Fact]
    public async Task DollarsToEuros_ReturnsCorrectConversion()
    {
        var converter = CreateConverter(FullApiResponse);
        var result = await converter.DollarsToEuros(10.8m);
        Assert.Equal(10m, result);
    }

    [Fact]
    public async Task EurosToDollars_ReturnsCorrectConversion()
    {
        var converter = CreateConverter(FullApiResponse);
        var result = await converter.EurosToDollars(10);
        Assert.Equal(10.8m, result);
    }

    [Fact]
    public async Task EurosToColones_WithColones_ReturnsCorrectConversion()
    {
        var converter = CreateConverter(FullApiResponse);
        var result = await converter.EurosToColones(10);
        Assert.Equal(5450m, result);
    }

    [Fact]
    public async Task ColonesToEuros_WithColones_ReturnsCorrectConversion()
    {
        var converter = CreateConverter(FullApiResponse);
        var result = await converter.ColonesToEuros(545m);
        Assert.Equal(1m, result, 10); // precision tolerance for decimal division
    }

    [Fact]
    public async Task EurosToColones_AfterHours_FallsBackToDollarCalculation()
    {
        var converter = CreateConverter(AfterHoursApiResponse);
        var result = await converter.EurosToColones(10);
        // Should use dolar.venta.valor * euro.valor = 515.50 * 1.08 = 556.74, then * 10 = 5567.4
        Assert.Equal(5567.4m, result);
    }

    [Fact]
    public async Task ColonesToEuros_AfterHours_FallsBackToDollarCalculation()
    {
        var converter = CreateConverter(AfterHoursApiResponse);
        var result = await converter.ColonesToEuros(556.74m);
        // Should use 1 / (dolar.compra.valor * euro.valor) = 1 / (505.25 * 1.08) = 1 / 545.67
        var expectedRate = 1m / (505.25m * 1.08m);
        Assert.Equal(556.74m * expectedRate, result);
    }

    [Fact]
    public async Task GetDollarExchangeRate_ReturnsCorrectValues()
    {
        var converter = CreateConverter(FullApiResponse);
        var (date, sale, purchase) = await converter.GetDollarExchangeRate();
        Assert.Equal("2025-01-15", date);
        Assert.Equal(515.50m, sale);
        Assert.Equal(505.25m, purchase);
    }

    [Fact]
    public async Task GetEuroExchangeRate_ReturnsCorrectValues()
    {
        var converter = CreateConverter(FullApiResponse);
        var (date, dollars, colones) = await converter.GetEuroExchangeRate();
        Assert.Equal("2025-01-15", date);
        Assert.Equal(1.08m, dollars);
        Assert.Equal(545.00m, colones);
    }

    [Fact]
    public async Task GetEuroExchangeRate_AfterHours_ReturnsNullColones()
    {
        var converter = CreateConverter(AfterHoursApiResponse);
        var (date, dollars, colones) = await converter.GetEuroExchangeRate();
        Assert.Equal("2025-01-15", date);
        Assert.Equal(1.08m, dollars);
        Assert.Null(colones);
    }

    [Fact]
    public async Task ConvertZeroAmount_ReturnsZero()
    {
        var converter = CreateConverter(FullApiResponse);
        Assert.Equal(0m, await converter.DollarsToColones(0));
        Assert.Equal(0m, await converter.ColonesToDollars(0));
        Assert.Equal(0m, await converter.DollarsToEuros(0));
        Assert.Equal(0m, await converter.EurosToDollars(0));
        Assert.Equal(0m, await converter.ColonesToEuros(0));
        Assert.Equal(0m, await converter.EurosToColones(0));
    }

    [Fact]
    public async Task NullApiResponse_ReturnsZero()
    {
        var converter = CreateConverter("{}");
        Assert.Equal(0m, await converter.DollarsToColones(100));
        Assert.Equal(0m, await converter.EurosToColones(100));
    }

    [Fact]
    public async Task Caching_ReusesResponse()
    {
        var callCount = 0;
        var handler = new CountingHandler(FullApiResponse, () => callCount++);
        var httpClient = new HttpClient(handler);
        var converter = new ColonesExchangeRate(httpClient, TimeSpan.FromMinutes(5));

        await converter.DollarsToColones(10);
        await converter.ColonesToDollars(10);
        await converter.GetDollarExchangeRate();

        Assert.Equal(1, callCount); // Only one HTTP call despite three conversions
    }

    [Fact]
    public async Task NoCaching_MakesMultipleCalls()
    {
        var callCount = 0;
        var handler = new CountingHandler(FullApiResponse, () => callCount++);
        var httpClient = new HttpClient(handler);
        var converter = new ColonesExchangeRate(httpClient, TimeSpan.Zero);

        await converter.DollarsToColones(10);
        await converter.ColonesToDollars(10);

        Assert.Equal(2, callCount);
    }

    [Fact]
    public async Task CancellationToken_IsRespected()
    {
        var converter = CreateConverter(FullApiResponse);
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => converter.DollarsToColones(10, cts.Token));
    }
}

/// <summary>
/// A test HTTP handler that counts how many requests were made.
/// </summary>
public class CountingHandler : DelegatingHandler
{
    private readonly string _responseJson;
    private readonly Action _onRequest;

    public CountingHandler(string responseJson, Action onRequest)
    {
        _responseJson = responseJson;
        _onRequest = onRequest;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _onRequest();
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(_responseJson, System.Text.Encoding.UTF8, "application/json")
        });
    }
}

/// <summary>
/// Integration tests that hit the live API. Require network connectivity.
/// </summary>
[Trait("Category", "Integration")]
public class CurrencyConverterIntegrationTests
{
    private readonly ColonesExchangeRate _colonesExchangeRate;
    public CurrencyConverterIntegrationTests()
    {
        _colonesExchangeRate = new ColonesExchangeRate();
    }

    [Fact]
    public async Task DollarsToColones_ShouldReturnPositiveValue()
    {
        var result = await _colonesExchangeRate.DollarsToColones(10);
        Assert.True(result > 0);
    }

    [Fact]
    public async Task ColonesToDollars_ShouldReturnPositiveValue()
    {
        var result = await _colonesExchangeRate.ColonesToDollars(10000);
        Assert.True(result > 0);
    }

    [Fact]
    public async Task DollarsToEuros_ShouldReturnPositiveValue()
    {
        var result = await _colonesExchangeRate.DollarsToEuros(10);
        Assert.True(result > 0);
    }

    [Fact]
    public async Task EurosToDollars_ShouldReturnPositiveValue()
    {
        var result = await _colonesExchangeRate.EurosToDollars(10);
        Assert.True(result > 0);
    }

    [Fact]
    public async Task EurosToColones_ShouldReturnPositiveValue()
    {
        var result = await _colonesExchangeRate.EurosToColones(10);
        Assert.True(result > 0);
    }

    [Fact]
    public async Task ColonesToEuros_ShouldReturnPositiveValue()
    {
        var result = await _colonesExchangeRate.ColonesToEuros(10000);
        Assert.True(result > 0);
    }

    [Fact]
    public async Task GetDollarExchangeRate_ShouldReturnValues()
    {
        var (date, sale, purchase) = await _colonesExchangeRate.GetDollarExchangeRate();
        Assert.NotNull(date);
        Assert.True(sale > 0);
        Assert.True(purchase > 0);
    }

    [Fact]
    public async Task GetEuroExchangeRate_ShouldReturnValues()
    {
        var (date, dollars, colones) = await _colonesExchangeRate.GetEuroExchangeRate();
        Assert.NotNull(date);
        Assert.True(dollars > 0);
        Assert.True(colones > 0 || colones == null);
    }
}