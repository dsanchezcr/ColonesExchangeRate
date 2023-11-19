# Colones Exchange Rate NuGet & npm Packages
This repository contains a NuGet & npm packages to provide currency conversion from Colones (Costa Rica - CRC ₡) to Dollars (United States - USD $) and Euros (European Union - EUR €). It consumes the [API from Ministerio de Hacienda de Costa Rica](https://api.hacienda.go.cr/indicadores/tc) (The API is in Spanish).

> Note: The API usually changes the response for the Euro exchange rate, during business hours, the response contains the exchange rate in dollars and colones, but after business hours, the response only contains the exchange rate in dollars. The ColonesExchangeRate packages handles this situation and returns the correct values.

[![ColonesExchangeRate - CI/CD](https://github.com/dsanchezcr/ColonesExchangeRate/actions/workflows/workflow.yaml/badge.svg)](https://github.com/dsanchezcr/ColonesExchangeRate/actions/workflows/workflow.yaml)

![](https://raw.githubusercontent.com/dsanchezcr/ColonesExchangeRate/main/images/Icon.png)

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
The result will look similar to this:
![Console Result](https://raw.githubusercontent.com/dsanchezcr/ColonesExchangeRate/main/images/ConsoleResult.jpg)

> Note: Replace amount with the amount of currency you want to convert.

The following methods return a tuple with three values: 
- GetDollarExchangeRate: the date of the exchange rate, the sale rate, and the purchase rate.
- GetEuroExchangeRate: the date of the exchange rate, the dollars rate, and the colones rate.

## npm Package

To install ColonesExchangeRate using npm, run the following command in the command line of your project's root directory:

```cli
npm i @dsanchezcr/colonesexchangerate
```

### Usage

To use ColonesExchangeRate, first import the colonesexchangerate module:
```javascript
import('@dsanchezcr/colonesexchangerate').then(module => {
     // Create a new instance of the class
    const exchangeRateClient = new module.default();
    // Use the methods of the class
    async function test() {
        try {
            const amount = 1000;
            const amountInColones = await exchangeRateClient.dollarsToColones(amount);
            console.log(`$${amount} is ₡${amountInColones}`);

            const amountInColonesToDollars = await exchangeRateClient.colonesToDollars(amount);
            console.log(`₡${amount} is $${amountInColonesToDollars}`);

            const amountInDollarsToEuros = await exchangeRateClient.dollarsToEuros(amount);
            console.log(`$${amount} is €${amountInDollarsToEuros}`);

            const amountInDollarsFromEuros = await exchangeRateClient.eurosToDollars(amount);
            console.log(`€${amount} is $${amountInDollarsFromEuros}`);

            const amountInColonesToEuros = await exchangeRateClient.colonesToEuros(amount);
            console.log(`₡${amount} is €${amountInColonesToEuros}`);

            const amountInEurosToColones = await exchangeRateClient.eurosToColones(amount);
            console.log(`€${amount} is ₡${amountInEurosToColones}`);

            const dollarExchangeRate = await exchangeRateClient.getDollarExchangeRate();
            console.log(`Dollar exchange rate: ${JSON.stringify(dollarExchangeRate)}`);

            const euroExchangeRate = await exchangeRateClient.getEuroExchangeRate();
            console.log(`Euro exchange rate: ${JSON.stringify(euroExchangeRate)}`);
        } catch (ex) {
            console.error(`Error: ${ex.message}`);
        }
    }
    test();
});
```
The result will look similar to this:
![npm console result](https://raw.githubusercontent.com/dsanchezcr/ColonesExchangeRate/main/images/npmConsoleResult.jpg)

> Note: Replace amount with the amount of currency you want to convert.

The following methods return a tuple with three values: 
- getDollarExchangeRate: the date of the exchange rate, the sale rate, and the purchase rate.
- getEuroExchangeRate: the date of the exchange rate, the dollars rate, and the colones rate.

# Contributing
Contributions are welcome! To contribute to ColonesExchangeRate, fork the repository and create a pull request with your changes.

# License
ColonesExchangeRate is licensed under the MIT License. See the [LICENSE](/LICENSE) file for details.
