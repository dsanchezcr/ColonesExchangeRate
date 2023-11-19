using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

// Class to handle exchange rates for Colones
public class ColonesExchangeRate
{
    // HttpClient instance to make web requests
    private static readonly HttpClient client = new HttpClient();

    // Method to get the exchange rate from the API
    private async Task<ConversionRate?> FetchExchangeRate()
    {
        // Fetch the exchange rate from the API
        var response = await client.GetStringAsync("https://api.hacienda.go.cr/indicadores/tc");

        // Set JSON deserialization options
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        // Deserialize the response and return
        return JsonSerializer.Deserialize<ConversionRate>(response, options);
    }

    // Method to convert currency using a provided rate selector
    private async Task<decimal> ConvertCurrency(decimal amount, Func<ConversionRate?, decimal> rateSelector)
    {
        // Get the exchange rate
        var rate = await FetchExchangeRate();

        // Convert the amount using the selected rate and return
        return amount * rateSelector(rate);
    }

    // Methods to convert between different currencies
    public Task<decimal> DollarsToColones(decimal amount) => ConvertCurrency(amount, rate => rate?.Dolar?.Venta?.Valor ?? 0);
    public Task<decimal> ColonesToDollars(decimal amount) => ConvertCurrency(amount, rate => 1 / (rate?.Dolar?.Compra?.Valor ?? 1));
    public Task<decimal> DollarsToEuros(decimal amount) => ConvertCurrency(amount, rate => 1 / (rate?.Euro?.Dolares ?? Decimal.Parse(rate?.Euro?.Valor ?? "0")));
    public Task<decimal> EurosToDollars(decimal amount) => ConvertCurrency(amount, rate => rate?.Euro?.Dolares ?? Decimal.Parse(rate?.Euro?.Valor ?? "0"));

    // Method to convert Colones to Euros
    public Task<decimal> ColonesToEuros(decimal amount) => ConvertCurrency(amount, rate =>
    {
        if (rate?.Euro?.Colones != null)
            return (decimal)(1 / rate.Euro.Colones);
        else
            return 1 / ((rate?.Dolar?.Compra?.Valor ?? 0) * Decimal.Parse(rate?.Euro?.Valor ?? "0"));
    });

    // Method to convert Euros to Colones
    public Task<decimal> EurosToColones(decimal amount) => ConvertCurrency(amount, rate =>
    {
        if (rate?.Euro?.Colones != null)
            return (decimal)rate.Euro.Colones;
        else
            return ((rate?.Dolar?.Venta?.Valor ?? 0) * Decimal.Parse(rate?.Euro?.Valor ?? "0"));
    });

    // Method to get the dollar exchange rate
    public async Task<(string? date, decimal sale, decimal purchase)> GetDollarExchangeRate()
    {
        var rate = await FetchExchangeRate();
        return (rate?.Dolar?.Venta?.Fecha, rate?.Dolar?.Venta?.Valor ?? 0, rate?.Dolar?.Compra?.Valor ?? 0);
    }

    // Method to get the euro exchange rate
    public async Task<(string? date, decimal? dollars, decimal? colones)> GetEuroExchangeRate()
    {
        var rate = await FetchExchangeRate();
        return (rate?.Euro?.Fecha, rate?.Euro?.Dolares ?? Decimal.Parse(rate?.Euro?.Valor ?? "0"), rate?.Euro?.Colones);
    }
}

// Classes to represent the conversion rate response
public class ConversionRate
{
    public Currency? Dolar { get; set; }
    public Euro? Euro { get; set; }
}

public class Currency
{
    public Rate? Venta { get; set; }
    public Rate? Compra { get; set; }
}

public class Euro
{
    public string? Fecha { get; set; }
    public decimal? Colones { get; set; }
    public decimal? Dolares { get; set; }
    public string? Valor { get; set; }
}

public class Rate
{
    public string? Fecha { get; set; }
    public decimal? Valor { get; set; }
}