using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class CurrencyConverter
{
    private static readonly HttpClient client = new HttpClient();
    public async Task<decimal> DollarsToColones(decimal amount)
    {
        try
        {
            var rate = await GetExchangeRate();
            return amount * (rate?.Dolar?.Venta?.Valor ?? 0);
        }
        catch (Exception ex)
        {
            throw new Exception("Error converting from dollars to colones. Details: ", ex);
        }
    }
    public async Task<decimal> ColonesToDollars(decimal amount)
    {
        try
        {
            var rate = await GetExchangeRate();
            return amount / (rate?.Dolar?.Compra?.Valor ?? 0);
        }
        catch (Exception ex)
        {
            throw new Exception("Error converting from colones to dollars. Details: ", ex);
        }
    }
    public async Task<decimal> DollarsToEuros(decimal amount)
    {
        try
        {
            var rate = await GetExchangeRate();
            return amount / (rate?.Euro?.Dolares ?? 0);
        }
        catch (Exception ex)
        {
            throw new Exception("Error converting from dollars to euros. Details: ", ex);
        }
    }

    public async Task<decimal> EurosToDollars(decimal amount)
    {
        try
        {
            var rate = await GetExchangeRate();
            return amount * (rate?.Euro?.Dolares ?? 0);
        }
        catch (Exception ex)
        {
            throw new Exception("Error converting from euros to dollars dollars. Details: ", ex);
        }
    }
    public async Task<decimal> ColonesToEuros(decimal amount) 
    {
        try 
        { 
            var rate = await GetExchangeRate();
            return amount / (rate?.Euro?.Colones ?? 0);
        }
        catch (Exception ex) 
        { 
            throw new Exception("Error converting from colones to euros. Details: ", ex); 
        } 
    }
    public async Task<decimal> EurosToColones(decimal amount)
    {
        try
        {
            var rate = await GetExchangeRate();
            return amount * (rate?.Euro?.Colones ?? 0);
        }
        catch (Exception ex)
        {
            throw new Exception("Error converting from euros to colones. Details: ", ex);
        }
    }
    public async Task<(string? date, decimal sale, decimal purchase)> GetDollarExchangeRate()
    {
        try
        {
            var rate = await GetExchangeRate();
            return (rate?.Dolar?.Venta?.Fecha, rate?.Dolar?.Venta?.Valor ?? 0, rate?.Dolar?.Compra?.Valor ?? 0);
        }
        catch (Exception ex)
        {
            throw new Exception("Error trying to get the dollar exchange rate. Details: ", ex);
        }
    }
    public async Task<(string? date, decimal dollars, decimal colones)> GetEuroExchangeRate()
    {
        try
        {
            var rate = await GetExchangeRate();
            return (rate?.Euro?.Fecha, rate?.Euro?.Dolares ?? 0, rate?.Euro?.Colones ?? 0);
        }
        catch (Exception ex)
        {
            throw new Exception("Error trying to get the euro exchange rate. Details: ", ex);
        }
    }
    private async Task<ConversionRate?> GetExchangeRate()
    {
        try
        {
            var response = await client.GetStringAsync("https://api.hacienda.go.cr/indicadores/tc");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var data = JsonSerializer.Deserialize<ConversionRate>(response, options);
            return data;
        }
        catch (Exception ex)
        {
            throw new Exception("Error trying to get the exchange rate from the API of Ministerio de Hacienda de Costa Rica https://api.hacienda.go.cr/indicadores/tc. Details: ", ex);
        }
    }
}
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
    public decimal Colones { get; set; }
    public decimal Dolares { get; set; }
}
public class Rate
{
    public string? Fecha { get; set; }
    public decimal Valor { get; set; }
}