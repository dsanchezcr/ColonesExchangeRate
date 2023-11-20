// Import axios for HTTP requests
import axios from 'axios';

// Class to handle exchange rates for Colones
class ColonesExchangeRate {
    // Method to get the exchange rate from the API
    async fetchExchangeRate() {
        // Fetch the exchange rate from the API
        const response = await axios.get('https://api.hacienda.go.cr/indicadores/tc');

        // Return the response data
        return response.data;
    }

    // Method to convert currency using a provided rate selector
    async convertCurrency(amount, rateSelector) {
        // Get the exchange rate
        const rate = await this.fetchExchangeRate();

        // Convert the amount using the selected rate and return
        return amount * rateSelector(rate);
    }

    // Methods to convert between different currencies
    dollarsToColones(amount) {
        return this.convertCurrency(amount, rate => rate?.dolar?.venta?.valor ?? 0);
    }

    colonesToDollars(amount) {
        return this.convertCurrency(amount, rate => 1 / (rate?.dolar?.compra?.valor ?? 1));
    }

    dollarsToEuros(amount) {
        return this.convertCurrency(amount, rate => 1 / (rate?.euro?.dolares ?? parseFloat(rate?.euro?.valor ?? "0")));
    }

    eurosToDollars(amount) {
        return this.convertCurrency(amount, rate => rate?.euro?.dolares ?? parseFloat(rate?.euro?.valor ?? "0"));
    }

    // Method to convert Colones to Euros
    colonesToEuros(amount) {
        return this.convertCurrency(amount, rate => {
            if (rate?.euro?.colones != null)
                return 1 / rate.euro.colones;
            else
                return 1 / ((rate?.dolar?.compra?.valor ?? 0) * parseFloat(rate?.euro?.valor ?? "0"));
        });
    }

    // Method to convert Euros to Colones
    eurosToColones(amount) {
        return this.convertCurrency(amount, rate => {
            if (rate?.euro?.colones != null)
                return rate.euro.colones;
            else {
                return (parseFloat(rate?.euro?.valor ?? "0") * (rate?.dolar?.venta?.valor ?? 0));
            }
        });
    }

    // Method to get the dollar exchange rate
    async getDollarExchangeRate() {
        const rate = await this.fetchExchangeRate();
        return {
            date: rate?.dolar?.venta?.fecha,
            sale: rate?.dolar?.venta?.valor ?? 0,
            purchase: rate?.dolar?.compra?.valor ?? 0
        };
    }

    // Method to get the euro exchange rate
    async getEuroExchangeRate() {
        const rate = await this.fetchExchangeRate();
        return {
            date: rate?.euro?.fecha,
            dollars: rate?.euro?.dolares ?? parseFloat(rate?.euro?.valor ?? "0"),
            colones: rate?.euro?.colones
        };
    }
}

// Export the class as a module
export default ColonesExchangeRate;