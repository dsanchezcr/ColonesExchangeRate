# ColonesExchangeRate NuGet Package
This NuGet package is a .NET library based on .NET Standard 2.1 that provides functionality for currency conversion from Colones (Costa Rica - CRC ₡) to Dollars (United States - USD $) and Euros (European Union - EUR €).

[![ColonesExchangeRate - CI/CD](https://github.com/dsanchezcr/ColonesExchangeRate/actions/workflows/workflow.yaml/badge.svg)](https://github.com/dsanchezcr/ColonesExchangeRate/actions/workflows/workflow.yaml)

![](https://raw.githubusercontent.com/dsanchezcr/ColonesExchangeRate/main/images/Icon.png)

# Information about the exchange rate
This NuGet package consumes the exchange rate through an API from Ministerio de Hacienda de Costa Rica, you can access the API in the following link: https://api.hacienda.go.cr/indicadores/tc (The API is in Spanish). 

# Installation
You can install the ColonesExchangeRate NuGet package using NuGet or GitHub Packages.

To install ColonesExchangeRate using NuGet, run the following command in the Package Manager Console:
```dotnetcli
Install-Package ColonesExchangeRate
```
# Usage
To use ColonesExchangeRate, first create an instance of the CurrencyConverter class:

```csharp
var converter = new CurrencyConverter();
```
Then, you can use the following methods to perform currency conversion:
```csharp
decimal dolaresAColones = await converter.DollarsToColones(amount);
decimal colonesADolares = await converter.ColonesToDollars(amount);
decimal dolaresAEuros = await converter.DollarsToEuros(amount);
decimal eurosADolares = await converter.EurosToDollars(amount);
decimal colonesAEuros = await converter.ColonesToEuros(amount);
decimal eurosAColones = await converter.EurosToColones(amount);
// true or false to get the date as part of the exchange rate information.
(DateTime? date, decimal sale, decimal purchase) dollarExchangeRate = await converter.GetDollarExchangeRate(true); 
(DateTime? date, decimal dollars, decimal colones) euroExchangeRate = await converter.GetEuroExchangeRate(true);
```
The result will look similar to this:
![Console Result](https://raw.githubusercontent.com/dsanchezcr/ColonesExchangeRate/main/images/ConsoleResult.jpg)

> Note: Replace amount with the amount of currency you want to convert.

The GetDollarExchangeRate and GetEuroExchangeRate methods return a tuple with three values: the date of the exchange rate, the sale rate, and the purchase rate.

# Contributing
Contributions are welcome! To contribute to ColonesExchangeRate, fork the repository and create a pull request with your changes.

# License
ColonesExchangeRate is licensed under the MIT License. See the [LICENSE](/LICENSE) file for details.
