using System.Collections.Generic;
using System.Threading.Tasks;
using Valutakalkulator_rest_api.Models.Requests;
using Valutakalkulator_rest_api.Models.Responses;

namespace Valutakalkulator_rest_api.Services
{
    public interface IConversionService
    {
        Task<ConversionResponse> RunConversion(ConversionRequest request);
        Task<List<string>> GetAvailableCurrencies();
        Task<LatestRatesResponse> GetLatestRates(string tilValuta, string fraValuta);
        GetRatesIntervalResponse GetRatesInterval(GetRatesIntervalRequest getRatesRequest);
    }
}
