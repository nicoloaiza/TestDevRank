using System;
using System.Threading.Tasks;

namespace Test.Currency
{
    public class PesoCurrencyQuery : ICurrrencyQuery
    {
        public PesoCurrencyQuery()
        {
        }

        public Task<string> QueryCurrencyValue()
        {
            throw new UnauthorizedAccessException();
        }
    }
}
