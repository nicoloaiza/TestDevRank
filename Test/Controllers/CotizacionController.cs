using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.Services;
using System.Net.Http;
using Test.Currency;

namespace Test.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CotizacionController : ControllerBase
    {
        private IDolarRequest _dolarRequest;
        private CurrencyQueryStrategyFactory currencyFactory;

        public CotizacionController(IDolarRequest dolarRequest)
        {
            _dolarRequest = dolarRequest;
            currencyFactory = new CurrencyQueryStrategyFactory(_dolarRequest);
        }

        [HttpGet("{currency}")]
        public async Task<ActionResult<string>> Get(string currency)
        {
            try
            {
                ICurrrencyQuery query = currencyFactory.getCurrencyQueryStrategy(currency);
                return Content(await query.QueryCurrencyValue());
            }
            catch (Exception) {
                return Unauthorized();
            }
        }
    }
}
