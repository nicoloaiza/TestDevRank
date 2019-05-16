using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Test.Services
{

    public class DolarRequest : IDolarRequest
    {
        public DolarRequest()
        {

        }

        public async Task<string> GetDolar()
        {
            HttpClient client = new HttpClient();
            var responseString = await client.GetStringAsync("http://www.bancoprovincia.com.ar/Principal/Dolar");

            return responseString;
        }
    }
}
