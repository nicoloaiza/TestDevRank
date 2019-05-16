using System;
using System.Threading.Tasks;
using Test.Services;

namespace Test.Currency
{
    public class DolarCurrencyQuery : ICurrrencyQuery
    {
        private IDolarRequest _dolarRequest;
        public DolarCurrencyQuery(IDolarRequest dolarRequest)
        {
            _dolarRequest = dolarRequest;
        }

        public async Task<string> QueryCurrencyValue()
        {
            return await _dolarRequest.GetDolar();
        }
    }
}
