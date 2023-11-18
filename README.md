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
(DateTime date, decimal sale, decimal purchase) dollarExchangeRate = await converter.GetDollarExchangeRate(); 
(DateTime date, decimal dollars, decimal colones) euroExchangeRate = await converter.GetEuroExchangeRate();
```
The result will look similar to this:
![Console Result](https://raw.githubusercontent.com/dsanchezcr/ColonesExchangeRate/main/images/ConsoleResult.jpg)

> Note: Replace amount with the amount of currency you want to convert.

The following methods return a tuple with three values: 
- GetDollarExchangeRate: the date of the exchange rate, the sale rate, and the purchase rate.
- GetEuroExchangeRate: the date of the exchange rate, the dollars rate, and the colones rate.

## npm Package

To install ColonesExchangeRate using npm, run the following command in the Package Manager Console:
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
            const amountInDollars = 100;
            const amountInColones = await exchangeRateClient.dollarsToColones(amountInDollars);
            console.log(`$${amountInDollars} is ₡${amountInColones}`);

            const amountInEuros = 100;
            const amountInDollarsFromEuros = await exchangeRateClient.eurosToDollars(amountInEuros);
            console.log(`€${amountInEuros} is $${amountInDollarsFromEuros}`);

            const amountInColonesToDollars = await exchangeRateClient.colonesToDollars(amountInColones);
            console.log(`₡${amountInColones} is $${amountInColonesToDollars}`);

            const amountInDollarsToEuros = await exchangeRateClient.dollarsToEuros(amountInDollars);
            console.log(`$${amountInDollars} is €${amountInDollarsToEuros}`);

            const amountInEurosToColones = await exchangeRateClient.eurosToColones(amountInEuros);
            console.log(`€${amountInEuros} is ₡${amountInEurosToColones}`);

            const amountInColonesToEuros = await exchangeRateClient.colonesToEuros(amountInColones);
            console.log(`₡${amountInColones} is €${amountInColonesToEuros}`);

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