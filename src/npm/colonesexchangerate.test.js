import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import ColonesExchangeRate from './colonesexchangerate.mjs';

describe('ColonesExchangeRate - Unit Tests', () => {
    it('should accept options in constructor', () => {
        const converter = new ColonesExchangeRate({ cacheTtlMs: 300000 });
        assert.ok(converter);
    });

    it('should accept empty constructor', () => {
        const converter = new ColonesExchangeRate();
        assert.ok(converter);
    });

    it('should have all expected public methods', () => {
        const converter = new ColonesExchangeRate();
        assert.equal(typeof converter.dollarsToColones, 'function');
        assert.equal(typeof converter.colonesToDollars, 'function');
        assert.equal(typeof converter.dollarsToEuros, 'function');
        assert.equal(typeof converter.eurosToDollars, 'function');
        assert.equal(typeof converter.colonesToEuros, 'function');
        assert.equal(typeof converter.eurosToColones, 'function');
        assert.equal(typeof converter.getDollarExchangeRate, 'function');
        assert.equal(typeof converter.getEuroExchangeRate, 'function');
    });

    it('should not expose fetchExchangeRate or convertCurrency as public', () => {
        const converter = new ColonesExchangeRate();
        assert.equal(converter.fetchExchangeRate, undefined);
        assert.equal(converter.convertCurrency, undefined);
    });
});

describe('ColonesExchangeRate - Integration Tests', { skip: !process.env.RUN_INTEGRATION_TESTS }, () => {
    const converter = new ColonesExchangeRate();
    const amount = 1000;

    it('should convert dollars to colones', async () => {
        const result = await converter.dollarsToColones(amount);
        assert.ok(Number(result) > 0, 'Dollars to colones conversion result should be greater than 0');
    });

    it('should convert colones to dollars', async () => {
        const result = await converter.colonesToDollars(amount);
        assert.ok(Number(result) > 0, 'Colones to dollars conversion result should be greater than 0');
    });

    it('should convert dollars to euros', async () => {
        const result = await converter.dollarsToEuros(amount);
        assert.ok(Number(result) > 0, 'Dollars to euros conversion result should be greater than 0');
    });

    it('should convert euros to dollars', async () => {
        const result = await converter.eurosToDollars(amount);
        assert.ok(Number(result) > 0, 'Euros to dollars conversion result should be greater than 0');
    });

    it('should convert colones to euros', async () => {
        const result = await converter.colonesToEuros(amount);
        assert.ok(Number(result) > 0, 'Colones to euros conversion result should be greater than 0');
    });

    it('should convert euros to colones', async () => {
        const result = await converter.eurosToColones(amount);
        assert.ok(Number(result) > 0, 'Euros to colones conversion result should be greater than 0');
    });

    it('should get dollar exchange rate with expected properties', async () => {
        const rate = await converter.getDollarExchangeRate();
        assert.ok(rate.hasOwnProperty('date'), 'Should have a date property');
        assert.ok(rate.hasOwnProperty('sale'), 'Should have a sale property');
        assert.ok(rate.hasOwnProperty('purchase'), 'Should have a purchase property');
        assert.ok(rate.sale > 0, 'Sale rate should be positive');
        assert.ok(rate.purchase > 0, 'Purchase rate should be positive');
    });

    it('should get euro exchange rate with expected properties', async () => {
        const rate = await converter.getEuroExchangeRate();
        assert.ok(rate.hasOwnProperty('date'), 'Should have a date property');
        assert.ok(rate.hasOwnProperty('dollars'), 'Should have a dollars property');
        assert.ok(rate.hasOwnProperty('colones'), 'Should have a colones property');
        assert.ok(rate.dollars > 0, 'Dollars rate should be positive');
    });

    it('should convert zero amount to zero', async () => {
        const result = await converter.dollarsToColones(0);
        assert.equal(result, 0);
    });
});