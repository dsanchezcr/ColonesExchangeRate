using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Provides currency conversion between Costa Rican Colones (CRC), US Dollars (USD), and Euros (EUR)
/// using exchange rates from the Ministerio de Hacienda de Costa Rica API.
/// </summary>
public class ColonesExchangeRate
{
    private static readonly HttpClient defaultClient = new HttpClient();
    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly HttpClient _client;
    private readonly TimeSpan _cacheDuration;
    private ConversionRate? _cachedRate;
    private DateTime _cacheExpiry = DateTime.MinValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColonesExchangeRate"/> class with no caching.
    /// </summary>
    public ColonesExchangeRate() : this(TimeSpan.Zero)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColonesExchangeRate"/> class with a configurable cache duration.
    /// </summary>
    /// <param name="cacheDuration">
    /// The duration to cache exchange rate data. Use <see cref="TimeSpan.Zero"/> to disable caching.
    /// A typical value is <c>TimeSpan.FromMinutes(5)</c>.
    /// </param>
    public ColonesExchangeRate(TimeSpan cacheDuration) : this(defaultClient, cacheDuration)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColonesExchangeRate"/> class with a custom <see cref="HttpClient"/> and cache duration.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> instance to use for API requests.</param>
    /// <param name="cacheDuration">
    /// The duration to cache exchange rate data. Use <see cref="TimeSpan.Zero"/> to disable caching.
    /// </param>
    public ColonesExchangeRate(HttpClient httpClient, TimeSpan cacheDuration)
    {
        _client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _cacheDuration = cacheDuration;
    }

    private async Task<ConversionRate?> FetchExchangeRate(CancellationToken cancellationToken = default)
    {
        if (_cacheDuration > TimeSpan.Zero && _cachedRate != null && DateTime.UtcNow < _cacheExpiry)
        {
            return _cachedRate;
        }

        using var responseMessage = await _client.GetAsync("https://api.hacienda.go.cr/indicadores/tc", cancellationToken);
        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.Content.ReadAsStringAsync();

        var rate = JsonSerializer.Deserialize<ConversionRate>(response, jsonOptions);

        if (_cacheDuration > TimeSpan.Zero)
        {
            _cachedRate = rate;
            _cacheExpiry = DateTime.UtcNow.Add(_cacheDuration);
        }

        return rate;
    }

    private async Task<decimal> ConvertCurrency(decimal amount, Func<ConversionRate?, decimal> rateSelector, CancellationToken cancellationToken = default)
    {
        var rate = await FetchExchangeRate(cancellationToken);
        return amount * rateSelector(rate);
    }

    private static decimal ParseDecimal(string? value)
    {
        return decimal.Parse(value ?? "0", CultureInfo.InvariantCulture);
    }

    private static decimal SafeDivide(decimal numerator, decimal denominator)
    {
        return denominator == 0 ? 0 : numerator / denominator;
    }

    /// <summary>Converts an amount from US Dollars to Costa Rican Colones.</summary>
    /// <param name="amount">The amount in US Dollars to convert.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The equivalent amount in Costa Rican Colones.</returns>
    public Task<decimal> DollarsToColones(decimal amount, CancellationToken cancellationToken = default) =>
        ConvertCurrency(amount, rate => rate?.Dolar?.Venta?.Valor ?? 0, cancellationToken);

    /// <summary>Converts an amount from Costa Rican Colones to US Dollars.</summary>
    /// <param name="amount">The amount in Costa Rican Colones to convert.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The equivalent amount in US Dollars.</returns>
    public Task<decimal> ColonesToDollars(decimal amount, CancellationToken cancellationToken = default) =>
        ConvertCurrency(amount, rate => SafeDivide(1, rate?.Dolar?.Compra?.Valor ?? 0), cancellationToken);

    /// <summary>Converts an amount from US Dollars to Euros.</summary>
    /// <param name="amount">The amount in US Dollars to convert.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The equivalent amount in Euros.</returns>
    public Task<decimal> DollarsToEuros(decimal amount, CancellationToken cancellationToken = default) =>
        ConvertCurrency(amount, rate => SafeDivide(1, rate?.Euro?.Dolares ?? ParseDecimal(rate?.Euro?.Valor)), cancellationToken);

    /// <summary>Converts an amount from Euros to US Dollars.</summary>
    /// <param name="amount">The amount in Euros to convert.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The equivalent amount in US Dollars.</returns>
    public Task<decimal> EurosToDollars(decimal amount, CancellationToken cancellationToken = default) =>
        ConvertCurrency(amount, rate => rate?.Euro?.Dolares ?? ParseDecimal(rate?.Euro?.Valor), cancellationToken);

    /// <summary>Converts an amount from Costa Rican Colones to Euros.</summary>
    /// <param name="amount">The amount in Costa Rican Colones to convert.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The equivalent amount in Euros.</returns>
    public Task<decimal> ColonesToEuros(decimal amount, CancellationToken cancellationToken = default) =>
        ConvertCurrency(amount, rate =>
        {
            if (rate?.Euro?.Colones != null && rate.Euro.Colones != 0)
                return SafeDivide(1, (decimal)rate.Euro.Colones);
            else
            {
                var dolarCompra = rate?.Dolar?.Compra?.Valor ?? 0;
                var euroValor = ParseDecimal(rate?.Euro?.Valor);
                return SafeDivide(1, dolarCompra * euroValor);
            }
        }, cancellationToken);

    /// <summary>Converts an amount from Euros to Costa Rican Colones.</summary>
    /// <param name="amount">The amount in Euros to convert.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The equivalent amount in Costa Rican Colones.</returns>
    public Task<decimal> EurosToColones(decimal amount, CancellationToken cancellationToken = default) =>
        ConvertCurrency(amount, rate =>
        {
            if (rate?.Euro?.Colones != null)
                return (decimal)rate.Euro.Colones;
            else
                return (rate?.Dolar?.Venta?.Valor ?? 0) * ParseDecimal(rate?.Euro?.Valor);
        }, cancellationToken);

    /// <summary>Gets the current US Dollar to Costa Rican Colones exchange rate.</summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A tuple containing the date, sale rate, and purchase rate.</returns>
    public async Task<(string? date, decimal sale, decimal purchase)> GetDollarExchangeRate(CancellationToken cancellationToken = default)
    {
        var rate = await FetchExchangeRate(cancellationToken);
        return (rate?.Dolar?.Venta?.Fecha, rate?.Dolar?.Venta?.Valor ?? 0, rate?.Dolar?.Compra?.Valor ?? 0);
    }

    /// <summary>Gets the current Euro exchange rate.</summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A tuple containing the date, dollar equivalent, and colones equivalent.</returns>
    public async Task<(string? date, decimal? dollars, decimal? colones)> GetEuroExchangeRate(CancellationToken cancellationToken = default)
    {
        var rate = await FetchExchangeRate(cancellationToken);
        return (rate?.Euro?.Fecha, rate?.Euro?.Dolares ?? ParseDecimal(rate?.Euro?.Valor), rate?.Euro?.Colones);
    }
}

/// <summary>Represents the full exchange rate response from the API.</summary>
public class ConversionRate
{
    /// <summary>Gets or sets the US Dollar exchange rate data.</summary>
    public Currency? Dolar { get; set; }
    /// <summary>Gets or sets the Euro exchange rate data.</summary>
    public Euro? Euro { get; set; }
}

/// <summary>Represents a currency with sale and purchase rates.</summary>
public class Currency
{
    /// <summary>Gets or sets the sale (venta) rate.</summary>
    public Rate? Venta { get; set; }
    /// <summary>Gets or sets the purchase (compra) rate.</summary>
    public Rate? Compra { get; set; }
}

/// <summary>Represents the Euro exchange rate data.</summary>
public class Euro
{
    /// <summary>Gets or sets the date of the exchange rate.</summary>
    public string? Fecha { get; set; }
    /// <summary>Gets or sets the exchange rate in Colones.</summary>
    public decimal? Colones { get; set; }
    /// <summary>Gets or sets the exchange rate in Dollars.</summary>
    public decimal? Dolares { get; set; }
    /// <summary>Gets or sets the raw value string from the API.</summary>
    public string? Valor { get; set; }
}

/// <summary>Represents a single exchange rate entry.</summary>
public class Rate
{
    /// <summary>Gets or sets the date of the rate.</summary>
    public string? Fecha { get; set; }
    /// <summary>Gets or sets the rate value.</summary>
    public decimal? Valor { get; set; }
}