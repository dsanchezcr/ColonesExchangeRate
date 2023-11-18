var _converter = new CurrencyConverter();
decimal amount = 1000;

var dolaresAColones = await _converter.DollarsToColones(amount);
Console.WriteLine($"{amount} dólares son {dolaresAColones} colones.");

var colonesADolares = await _converter.ColonesToDollars(amount);
Console.WriteLine($"{amount} colones son {colonesADolares} dólares.");

var dolaresAEuros = await _converter.DollarsToEuros(amount);
Console.WriteLine($"{amount} dólares son {dolaresAEuros} euros.");

var eurosADolares = await _converter.EurosToDollars(amount);
Console.WriteLine($"{amount} euros son {eurosADolares} dólares.");

var colonesAEuros = await _converter.ColonesToEuros(amount);
Console.WriteLine($"{amount} colones son {colonesAEuros} euros.");

var eurosAColones = await _converter.EurosToColones(amount);
Console.WriteLine($"{amount} euros son {eurosAColones} colones.");

var (date, sale, purchase) = await _converter.GetDollarExchangeRate();
Console.WriteLine($"Dólar - Fecha: {date}, Venta: {sale}, Compra: {purchase}");

var (_date, dollars, colones) = await _converter.GetEuroExchangeRate();
Console.WriteLine($"Euro - Fecha: {_date}, Dolares: {dollars}, Colones: {colones}");