const colonesexchangerate = require('./colonesexchangerate');

describe('colonesexchangerate', () => {
  let converter;

  beforeEach(() => {
    converter = new colonesexchangerate();
  });

  test('converts dollars to colones', async () => {
    const amount = 100;
    const result = await converter.dollarsToColones(amount);
    expect(Number(result)).toBeGreaterThan(0);
  });

  test('converts colones to dollars', async () => {
    const amount = 1000;
    const result = await converter.colonesToDollars(amount);
    expect(Number(result)).toBeGreaterThan(0);
  });

  test('converts dollars to euros', async () => {
    const amount = 100;
    const result = await converter.dollarsToEuros(amount);
    expect(Number(result)).toBeGreaterThan(0);
  });

  test('converts euros to dollars', async () => {
    const amount = 100;
    const result = await converter.eurosToDollars(amount);
    expect(Number(result)).toBeGreaterThan(0);
  });

  test('converts colones to euros', async () => {
    const amount = 1000;
    const result = await converter.colonesToEuros(amount);
    expect(Number(result)).toBeGreaterThan(0);
  });

  test('converts euros to colones', async () => {
    const amount = 100;
    const result = await converter.eurosToColones(amount);
    expect(Number(result)).toBeGreaterThan(0);
  });

  test('gets dollar exchange rate', async () => {
    const rate = await converter.getDollarExchangeRate();
    expect(rate).toHaveProperty('sale');
    expect(rate).toHaveProperty('purchase');
  });

  test('gets euro exchange rate', async () => {
    const rate = await converter.getEuroExchangeRate();
    expect(rate).toHaveProperty('dollars');
  });
});