var converter = new ColonesExchangeRate();
var amount = 1000;

decimal dollarsToColones = await converter.DollarsToColones(amount);
decimal colonesToDollars = await converter.ColonesToDollars(amount);
decimal dollarsToEuros = await converter.DollarsToEuros(amount);
decimal eurosToDollars = await converter.EurosToDollars(amount);
decimal colonesToEuros = await converter.ColonesToEuros(amount);
decimal eurosToColones = await converter.EurosToColones(amount);
var dollarExchangeRate = await converter.GetDollarExchangeRate();
var euroExchangeRate = await converter.GetEuroExchangeRate();

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