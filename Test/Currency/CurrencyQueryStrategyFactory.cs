using System;
using Test.Services;

namespace Test.Currency
{
    public class CurrencyQueryStrategyFactory
    {

        private ICurrrencyQuery dolarCurrency;
        private ICurrrencyQuery pesoCurrency;
        private ICurrrencyQuery realCurrency;
        private IDolarRequest _dolarRequest;

        public CurrencyQueryStrategyFactory(IDolarRequest dolarRequest)
        {
            _dolarRequest = dolarRequest;
            dolarCurrency = new DolarCurrencyQuery(_dolarRequest);
            pesoCurrency = new PesoCurrencyQuery();
            realCurrency = new RealCurrencyQuery();
        }

        public ICurrrencyQuery getCurrencyQueryStrategy(string currency)
        {
            switch (currency.ToUpper())
            {
                case "DOLAR": return dolarCurrency;
                case "PESO": return pesoCurrency;
                case "REAL": return realCurrency;
                default: return null;
            }
        }
    }
}
