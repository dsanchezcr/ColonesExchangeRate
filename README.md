# Colones Exchange Rate NuGet & npm Packages
This repository contains a NuGet & npm packages to provide currency conversion from Colones (Costa Rica - CRC ₡) to Dollars (United States - USD $) and Euros (European Union - EUR €). It consumes the [API from Ministerio de Hacienda de Costa Rica](https://api.hacienda.go.cr/indicadores/tc) (The API is in Spanish).

[![ColonesExchangeRate - CI/CD](https://github.com/dsanchezcr/ColonesExchangeRate/actions/workflows/workflow.yaml/badge.svg)](https://github.com/dsanchezcr/ColonesExchangeRate/actions/workflows/workflow.yaml)

![](https://raw.githubusercontent.com/dsanchezcr/ColonesExchangeRate/main/images/Icon.png)

# Installation
You can install the ColonesExchangeRate package from [NuGet.org](https://www.npmjs.com/package/@dsanchezcr/colonesexchangerate), [npmjs.com](https://www.npmjs.com/package/@dsanchezcr/colonesexchangerate) or [GitHub Packages](https://github.com/dsanchezcr?tab=packages&repo_name=ColonesExchangeRate).


## NuGet Package
To install ColonesExchangeRate using NuGet, run the following command in the Package Manager Console:
```dotnetcli
Install-Package ColonesExchangeRate
```
### Usage
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

## npm Package

To install ColonesExchangeRate using npm, run the following command in the Package Manager Console:
```cli
npm install @dsanchezcr/colones-exchange-rate
```

### Usage

To use ColonesExchangeRate, first import the CurrencyConverter class:
```javascript
import { CurrencyConverter } from '@dsanchezcr/colones-exchange-rate';
```
Then, you can use the following methods to perform currency conversion:
```javascript
const converter = new CurrencyConverter();
const dolaresAColones = await converter.DollarsToColones(amount);
const colonesADolares = await converter.ColonesToDollars(amount);
const dolaresAEuros = await converter.DollarsToEuros(amount);
const eurosADolares = await converter.EurosToDollars(amount);
const colonesAEuros = await converter.ColonesToEuros(amount);
const eurosAColones = await converter.EurosToColones(amount);
// true or false to get the date as part of the exchange rate information.
const dollarExchangeRate = await converter.GetDollarExchangeRate(true);
const euroExchangeRate = await converter.GetEuroExchangeRate(true);
```

> Note: Replace amount with the amount of currency you want to convert.

The GetDollarExchangeRate and GetEuroExchangeRate methods return a tuple with three values: the date of the exchange rate, the sale rate, and the purchase rate.

# Contributing
Contributions are welcome! To contribute to ColonesExchangeRate, fork the repository and create a pull request with your changes.

# License
ColonesExchangeRate is licensed under the MIT License. See the [LICENSE](/LICENSE) file for details.
