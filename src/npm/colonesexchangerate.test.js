import assert from 'assert';
import colonesexchangerate from './colonesexchangerate.js';

let converter = new colonesexchangerate();

async function runTests() {
  try {
    const amount = 100;
    let result = await converter.dollarsToColones(amount);
    console.log(`Dollars to colones: ${result}`);
    assert(Number(result) > 0, 'Dollars to colones conversion result should be greater than 0');

    result = await converter.colonesToDollars(amount);
    console.log(`Colones to dollars: ${result}`);
    assert(Number(result) > 0, 'Colones to dollars conversion result should be greater than 0');

    result = await converter.dollarsToEuros(amount);
    console.log(`Dollars to euros: ${result}`);
    assert(Number(result) > 0, 'Dollars to euros conversion result should be greater than 0');

    result = await converter.eurosToDollars(amount);
    console.log(`Euros to dollars: ${result}`);
    assert(Number(result) > 0, 'Euros to dollars conversion result should be greater than 0');

    result = await converter.colonesToEuros(amount);
    console.log(`Colones to euros: ${result}`);
    assert(Number(result) > 0, 'Colones to euros conversion result should be greater than 0');

    result = await converter.eurosToColones(amount);
    console.log(`Euros to colones: ${result}`);
    assert(Number(result) > 0, 'Euros to colones conversion result should be greater than 0');

    let rate = await converter.getDollarExchangeRate();
    console.log(`Dollar exchange rate: ${JSON.stringify(rate)}`);
    assert(rate.hasOwnProperty('date'), 'Dollar exchange rate should have a date property');
    assert(rate.hasOwnProperty('sale'), 'Dollar exchange rate should have a sale property');
    assert(rate.hasOwnProperty('purchase'), 'Dollar exchange rate should have a purchase property');

    rate = await converter.getEuroExchangeRate();
    console.log(`Euro exchange rate: ${JSON.stringify(rate)}`);
    assert(rate.hasOwnProperty('date'), 'Euro exchange rate should have a date property');
    assert(rate.hasOwnProperty('dollars'), 'Euro exchange rate should have a dollars property');
    assert(rate.hasOwnProperty('colones'), 'Euro exchange rate should have a colones property');

    console.log('All tests passed!');
  } catch (error) {
    console.error(error);
    process.exit(1);
  }
}

runTests();