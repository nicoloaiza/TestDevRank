using System;
using System.Threading.Tasks;

namespace Test.Services
{
    public interface IDolarRequest
    {
        Task<string> GetDolar();
    }
}
