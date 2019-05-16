using System;
using System.Threading.Tasks;

namespace Test.Currency
{
    public class RealCurrencyQuery : ICurrrencyQuery
    {
        public RealCurrencyQuery()
        {
        }

        public Task<string> QueryCurrencyValue()
        {
            throw new UnauthorizedAccessException();
        }
    }
}
