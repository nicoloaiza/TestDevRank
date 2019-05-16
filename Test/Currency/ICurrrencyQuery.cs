using System;
using System.Threading.Tasks;

namespace Test.Currency
{
    public interface ICurrrencyQuery
    {
        Task<string> QueryCurrencyValue();
    }
}
