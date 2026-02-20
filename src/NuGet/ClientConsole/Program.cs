using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

// Create a converter with 5-minute caching to avoid redundant API calls
var converter = new ColonesExchangeRate(TimeSpan.FromMinutes(5));
var amount = 1000;

decimal dollarsToColones = await converter.DollarsToColones(amount, cts.Token);
decimal colonesToDollars = await converter.ColonesToDollars(amount, cts.Token);
decimal dollarsToEuros = await converter.DollarsToEuros(amount, cts.Token);
decimal eurosToDollars = await converter.EurosToDollars(amount, cts.Token);
decimal colonesToEuros = await converter.ColonesToEuros(amount, cts.Token);
decimal eurosToColones = await converter.EurosToColones(amount, cts.Token);
var dollarExchangeRate = await converter.GetDollarExchangeRate(cts.Token);
var euroExchangeRate = await converter.GetEuroExchangeRate(cts.Token);

Console.WriteLine($"{amount} Dollars = {dollarsToColones} Colones");
Console.WriteLine($"{amount} Colones = {colonesToDollars} Dollars");
Console.WriteLine($"{amount} Dollars = {dollarsToEuros} Euros");
Console.WriteLine($"{amount} Euros = {eurosToDollars} Dollars");
Console.WriteLine($"{amount} Colones = {colonesToEuros} Euros");
Console.WriteLine($"{amount} Euros = {eurosToColones} Colones");
    
Console.WriteLine($"Dollar exchange rate: {dollarExchangeRate.date} - Sale: {dollarExchangeRate.sale} - Purchase: {dollarExchangeRate.purchase}");
if (euroExchangeRate.colones != null)
    Console.WriteLine($"Euro exchange rate: {euroExchangeRate.date} - Dollars: {euroExchangeRate.dollars} - Colones: {euroExchangeRate.colones}");
else
    Console.WriteLine($"Euro exchange rate: {euroExchangeRate.date} - Dollars: {euroExchangeRate.dollars}");