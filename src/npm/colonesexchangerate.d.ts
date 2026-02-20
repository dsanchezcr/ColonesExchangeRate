export interface DollarExchangeRate {
    /** The date of the exchange rate. */
    date: string | undefined;
    /** The sale (venta) rate in Colones per Dollar. */
    sale: number;
    /** The purchase (compra) rate in Colones per Dollar. */
    purchase: number;
}

export interface EuroExchangeRate {
    /** The date of the exchange rate. */
    date: string | undefined;
    /** The exchange rate in Dollars per Euro. */
    dollars: number;
    /** The exchange rate in Colones per Euro (null after business hours). */
    colones: number | null | undefined;
}

export interface ColonesExchangeRateOptions {
    /** Cache duration in milliseconds. Use 0 to disable caching. Default: 0. */
    cacheTtlMs?: number;
}

/**
 * Provides currency conversion between Costa Rican Colones (CRC), US Dollars (USD), and Euros (EUR)
 * using exchange rates from the Ministerio de Hacienda de Costa Rica API.
 */
declare class ColonesExchangeRate {
    /**
     * Creates a new ColonesExchangeRate instance.
     * @param options - Configuration options.
     */
    constructor(options?: ColonesExchangeRateOptions);

    /** Converts an amount from US Dollars to Costa Rican Colones. */
    dollarsToColones(amount: number): Promise<number>;

    /** Converts an amount from Costa Rican Colones to US Dollars. */
    colonesToDollars(amount: number): Promise<number>;

    /** Converts an amount from US Dollars to Euros. */
    dollarsToEuros(amount: number): Promise<number>;

    /** Converts an amount from Euros to US Dollars. */
    eurosToDollars(amount: number): Promise<number>;

    /** Converts an amount from Costa Rican Colones to Euros. */
    colonesToEuros(amount: number): Promise<number>;

    /** Converts an amount from Euros to Costa Rican Colones. */
    eurosToColones(amount: number): Promise<number>;

    /** Gets the current US Dollar to Costa Rican Colones exchange rate. */
    getDollarExchangeRate(): Promise<DollarExchangeRate>;

    /** Gets the current Euro exchange rate. */
    getEuroExchangeRate(): Promise<EuroExchangeRate>;
}

export default ColonesExchangeRate;
