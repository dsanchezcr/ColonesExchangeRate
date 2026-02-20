// Import axios for HTTP requests
import axios from 'axios';

// Class to handle exchange rates for Colones
class ColonesExchangeRate {
    #cachedRate = null;
    #cacheExpiry = 0;
    #cacheTtlMs = 0;

    /**
     * Creates a new ColonesExchangeRate instance.
     * @param {object} [options] - Configuration options.
     * @param {number} [options.cacheTtlMs=0] - Cache duration in milliseconds. Use 0 to disable caching.
     */
    constructor(options = {}) {
        this.#cacheTtlMs = options.cacheTtlMs ?? 0;
    }

    // Private method to get the exchange rate from the API
    async #fetchExchangeRate() {
        if (this.#cacheTtlMs > 0 && this.#cachedRate && Date.now() < this.#cacheExpiry) {
            return this.#cachedRate;
        }

        const response = await axios.get('https://api.hacienda.go.cr/indicadores/tc');
        const data = response.data;

        if (this.#cacheTtlMs > 0) {
            this.#cachedRate = data;
            this.#cacheExpiry = Date.now() + this.#cacheTtlMs;
        }

        return data;
    }

    // Helper to avoid division by zero
    static #safeDivide(numerator, denominator) {
        return denominator === 0 ? 0 : numerator / denominator;
    }

    // Private method to convert currency using a provided rate selector
    async #convertCurrency(amount, rateSelector) {
        const rate = await this.#fetchExchangeRate();
        return amount * rateSelector(rate);
    }

    // Methods to convert between different currencies
    dollarsToColones(amount) {
        return this.#convertCurrency(amount, rate => rate?.dolar?.venta?.valor ?? 0);
    }

    colonesToDollars(amount) {
        return this.#convertCurrency(amount, rate =>
            ColonesExchangeRate.#safeDivide(1, rate?.dolar?.compra?.valor ?? 0));
    }

    dollarsToEuros(amount) {
        return this.#convertCurrency(amount, rate =>
            ColonesExchangeRate.#safeDivide(1, rate?.euro?.dolares ?? parseFloat(rate?.euro?.valor ?? "0")));
    }

    eurosToDollars(amount) {
        return this.#convertCurrency(amount, rate => rate?.euro?.dolares ?? parseFloat(rate?.euro?.valor ?? "0"));
    }

    // Method to convert Colones to Euros
    colonesToEuros(amount) {
        return this.#convertCurrency(amount, rate => {
            if (rate?.euro?.colones != null && rate.euro.colones !== 0)
                return ColonesExchangeRate.#safeDivide(1, rate.euro.colones);
            else {
                const dolarCompra = rate?.dolar?.compra?.valor ?? 0;
                const euroValor = parseFloat(rate?.euro?.valor ?? "0");
                return ColonesExchangeRate.#safeDivide(1, dolarCompra * euroValor);
            }
        });
    }

    // Method to convert Euros to Colones
    eurosToColones(amount) {
        return this.#convertCurrency(amount, rate => {
            if (rate?.euro?.colones != null)
                return rate.euro.colones;
            else {
                return (parseFloat(rate?.euro?.valor ?? "0") * (rate?.dolar?.venta?.valor ?? 0));
            }
        });
    }

    // Method to get the dollar exchange rate
    async getDollarExchangeRate() {
        const rate = await this.#fetchExchangeRate();
        return {
            date: rate?.dolar?.venta?.fecha,
            sale: rate?.dolar?.venta?.valor ?? 0,
            purchase: rate?.dolar?.compra?.valor ?? 0
        };
    }

    // Method to get the euro exchange rate
    async getEuroExchangeRate() {
        const rate = await this.#fetchExchangeRate();
        return {
            date: rate?.euro?.fecha,
            dollars: rate?.euro?.dolares ?? parseFloat(rate?.euro?.valor ?? "0"),
            colones: rate?.euro?.colones
        };
    }
}

// Export the class as a module
export default ColonesExchangeRate;