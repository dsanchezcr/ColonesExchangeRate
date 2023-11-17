const axios = require('axios');

class colonesexchangerate {
  constructor() {
    this.client = axios;
  }

  async getExchangeRate() {
    try {
      const response = await this.client.get('https://api.hacienda.go.cr/indicadores/tc');
      return response.data;
    } catch (ex) {
      throw new Error(`Error trying to get the exchange rate from the API. Details: ${ex}`);
    }
  }

  async dollarsToColones(amount) {
    try {
      const rate = await this.getExchangeRate();
      return (amount * (rate?.dolar?.venta?.valor || 0)).toFixed(2);
    } catch (ex) {
      throw new Error(`Error converting from dollars to colones. Details: ${ex}`);
    }
  }

  async colonesToDollars(amount) {
    try {
      const rate = await this.getExchangeRate();
      return (amount / (rate?.dolar?.compra?.valor || 0)).toFixed(2);
    } catch (ex) {
      throw new Error(`Error converting from colones to dollars. Details: ${ex}`);
    }
  }

  async dollarsToEuros(amount) {
    try {
      const rate = await this.getExchangeRate();
      return (amount / (rate?.euro?.dolares || 0)).toFixed(2);
    } catch (ex) {
      throw new Error(`Error converting from dollars to euros. Details: ${ex}`);
    }
  }

  async eurosToDollars(amount) {
    try {
      const rate = await this.getExchangeRate();
      return (amount * (rate?.euro?.dolares || 0)).toFixed(2);
    } catch (ex) {
      throw new Error(`Error converting from euros to dollars. Details: ${ex}`);
    }
  }

  async colonesToEuros(amount) {
    try {
      const rate = await this.getExchangeRate();
      return (amount / (rate?.euro?.colones || 0)).toFixed(2);
    } catch (ex) {
      throw new Error(`Error converting from colones to euros. Details: ${ex}`);
    }
  }

  async eurosToColones(amount) {
    try {
      const rate = await this.getExchangeRate();
      return (amount * (rate?.euro?.colones || 0)).toFixed(2);
    } catch (ex) {
      throw new Error(`Error converting from euros to colones. Details: ${ex}`);
    }
  }

  async getDollarExchangeRate(includeDate = false) {
    try {
      const rate = await this.getExchangeRate();
      return {
        date: includeDate ? rate?.dolar?.venta?.fecha : null,
        sale: rate?.dolar?.venta?.valor || 0,
        purchase: rate?.dolar?.compra?.valor || 0
      };
    } catch (ex) {
      throw new Error(`Error trying to get the dollar exchange rate. Details: ${ex}`);
    }
  }

  async getEuroExchangeRate(includeDate = false) {
    try {
      const rate = await this.getExchangeRate();
      return {
        date: includeDate ? rate?.euro?.fecha : null,
        dollars: rate?.euro?.valor || 0
      };
    } catch (ex) {
      throw new Error(`Error trying to get the euro exchange rate. Details: ${ex}`);
    }
  }
}

module.exports = colonesexchangerate;