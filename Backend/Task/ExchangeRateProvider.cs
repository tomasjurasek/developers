﻿using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public partial class ExchangeRateProvider
    {
        private readonly IExchangeRateClient exchangeRateClient;

        public ExchangeRateProvider(IExchangeRateClient exchangeRateClient)
        {
            this.exchangeRateClient = exchangeRateClient;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async IAsyncEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            await foreach (var exchange in exchangeRateClient.GetExchanges(currencies))
            {
                var newRate = exchange.rate / exchange.amout;

                yield return new ExchangeRate(new Currency(exchange.code), new Currency("CZK"), newRate);
            }
        }
    }
}
