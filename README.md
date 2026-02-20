# Colones Exchange Rate NuGet & npm Packages
This repository contains a NuGet & npm packages to provide currency conversion from Colones (Costa Rica - CRC ₡) to Dollars (United States - USD $) and Euros (European Union - EUR €). It consumes the [API from Ministerio de Hacienda de Costa Rica](https://api.hacienda.go.cr/indicadores/tc) (The API is in Spanish).

> Note: The API usually changes the response for the Euro exchange rate, during business hours, the response contains the exchange rate in dollars and colones, but after business hours, the response only contains the exchange rate in dollars. The ColonesExchangeRate packages handles this situation and returns the correct values.

[![ColonesExchangeRate - CI/CD](https://github.com/dsanchezcr/ColonesExchangeRate/actions/workflows/workflow.yaml/badge.svg)](https://github.com/dsanchezcr/ColonesExchangeRate/actions/workflows/workflow.yaml)
[![NuGet](https://img.shields.io/nuget/v/ColonesExchangeRate)](https://www.nuget.org/packages/ColonesExchangeRate)
[![npm](https://img.shields.io/npm/v/@dsanchezcr/colonesexchangerate)](https://www.npmjs.com/package/@dsanchezcr/colonesexchangerate)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

![](https://raw.githubusercontent.com/dsanchezcr/ColonesExchangeRate/main/images/Icon.png)

## Prerequisites
- **NuGet**: .NET 10.0 or later (library targets .NET Standard 2.1)
- **npm**: Node.js 18.0.0 or later

# Installation
You can install the ColonesExchangeRate package from [NuGet.org](https://www.nuget.org/packages/ColonesExchangeRate), [npmjs.com](https://www.npmjs.com/package/@dsanchezcr/colonesexchangerate) or [GitHub Packages](https://github.com/dsanchezcr?tab=packages&repo_name=ColonesExchangeRate).


## NuGet Package
To install ColonesExchangeRate using NuGet, run the following command in the Package Manager Console:
```dotnetcli
Install-Package ColonesExchangeRate
```
### Usage
To use ColonesExchangeRate, first create an instance of the class:

```csharp
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
```

#### Caching (Optional)
To avoid redundant API calls when performing multiple conversions, enable caching:

```csharp
// Cache exchange rates for 5 minutes
var converter = new ColonesExchangeRate(TimeSpan.FromMinutes(5));
```

#### CancellationToken Support
All methods accept an optional `CancellationToken`:

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
var result = await converter.DollarsToColones(100, cts.Token);
```

#### Custom HttpClient
You can inject your own `HttpClient` (e.g. for use with `IHttpClientFactory`):

```csharp
var converter = new ColonesExchangeRate(httpClient, TimeSpan.FromMinutes(5));
```
The result will look similar to this:
![Console Result](https://raw.githubusercontent.com/dsanchezcr/ColonesExchangeRate/main/images/ConsoleResult.jpg)

> Note: Replace amount with the amount of currency you want to convert.

## npm Package

To install ColonesExchangeRate using npm, run the following command in the command line of your project's root directory:

```cli
npm i @dsanchezcr/colonesexchangerate
```

### Usage

To use ColonesExchangeRate, import the module:
```javascript
import ColonesExchangeRate from '@dsanchezcr/colonesexchangerate';

const converter = new ColonesExchangeRate();
const amount = 1000;

const amountInColones = await converter.dollarsToColones(amount);
console.log(`$${amount} is ₡${amountInColones}`);

const amountInDollars = await converter.colonesToDollars(amount);
console.log(`₡${amount} is $${amountInDollars}`);

const amountInEuros = await converter.dollarsToEuros(amount);
console.log(`$${amount} is €${amountInEuros}`);

const amountFromEuros = await converter.eurosToDollars(amount);
console.log(`€${amount} is $${amountFromEuros}`);

const colonesToEuros = await converter.colonesToEuros(amount);
console.log(`₡${amount} is €${colonesToEuros}`);

const eurosToColones = await converter.eurosToColones(amount);
console.log(`€${amount} is ₡${eurosToColones}`);

const dollarRate = await converter.getDollarExchangeRate();
console.log(`Dollar exchange rate: ${JSON.stringify(dollarRate)}`);

const euroRate = await converter.getEuroExchangeRate();
console.log(`Euro exchange rate: ${JSON.stringify(euroRate)}`);
```

#### Caching (Optional)
To avoid redundant API calls when performing multiple conversions, enable caching:

```javascript
// Cache exchange rates for 5 minutes
const converter = new ColonesExchangeRate({ cacheTtlMs: 300000 });
```

#### TypeScript
The package includes TypeScript type definitions out of the box — no `@types` package needed.
The result will look similar to this:
![npm console result](https://raw.githubusercontent.com/dsanchezcr/ColonesExchangeRate/main/images/npmConsoleResult.jpg)

> Note: Replace amount with the amount of currency you want to convert.

The following methods return an object/tuple with exchange rate details: 
- `getDollarExchangeRate` / `GetDollarExchangeRate`: the date of the exchange rate, the sale rate, and the purchase rate.
- `getEuroExchangeRate` / `GetEuroExchangeRate`: the date of the exchange rate, the dollars rate, and the colones rate.

# API Reference

| Method | Parameters | Returns | Description |
|--------|-----------|---------|-------------|
| `DollarsToColones` / `dollarsToColones` | `amount` | `decimal` / `number` | Converts USD to CRC |
| `ColonesToDollars` / `colonesToDollars` | `amount` | `decimal` / `number` | Converts CRC to USD |
| `DollarsToEuros` / `dollarsToEuros` | `amount` | `decimal` / `number` | Converts USD to EUR |
| `EurosToDollars` / `eurosToDollars` | `amount` | `decimal` / `number` | Converts EUR to USD |
| `ColonesToEuros` / `colonesToEuros` | `amount` | `decimal` / `number` | Converts CRC to EUR |
| `EurosToColones` / `eurosToColones` | `amount` | `decimal` / `number` | Converts EUR to CRC |
| `GetDollarExchangeRate` / `getDollarExchangeRate` | — | `(date, sale, purchase)` / `{date, sale, purchase}` | Gets current USD/CRC rate |
| `GetEuroExchangeRate` / `getEuroExchangeRate` | — | `(date, dollars, colones)` / `{date, dollars, colones}` | Gets current EUR rate |

> All NuGet methods also accept an optional `CancellationToken` parameter.

# Contributing
Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for development setup, testing instructions, and guidelines.

# License
ColonesExchangeRate is licensed under the MIT License. See the [LICENSE](/LICENSE) file for details.
