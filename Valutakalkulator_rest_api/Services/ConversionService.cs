using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Valutakalkulator_rest_api.Models.Requests;
using Valutakalkulator_rest_api.Models.Responses;
using Valutakalkulator_rest_api.Repositories;

namespace Valutakalkulator_rest_api.Services
{
    public class ConversionService : IConversionService
    {
        private readonly HttpClient _client;
        private const string _urlSymbols = "symbols";
        private const string _urlLatest = "latest?symbols={0}&base={1}";
        private const string _urlConvert = "convert?to={0}&from={1}&amount={2}&date={3}";
        private readonly ICurrencyRepository _currencyRepository;

        public ConversionService(HttpClient client, ICurrencyRepository currencyRepository)
        {
            _client = client;
            _currencyRepository = currencyRepository;
        }

        public GetRatesIntervalResponse GetRatesInterval(GetRatesIntervalRequest getRatesRequest)
        {
            var fraValuta = getRatesRequest.FraValuta.ToUpper();
            var result = _currencyRepository.GetRatesInterval(fraValuta,
                DateTime.Parse(getRatesRequest.FraDato), DateTime.Parse(getRatesRequest.TilDato)).ToList();

            return new GetRatesIntervalResponse()
            {
                Base = fraValuta,
                Items = result.Select(x => new Item()
                {
                    Date = x.Date.ToString("yyyy-MM-dd"),
                    Timestamp = x.Timestamp,
                    Rates = x.Rates.ToDictionary(y => y.Code, y => y.ExchangeRate)
                }).ToList()
            };
        }

        public async Task<ConversionResponse> RunConversion(ConversionRequest request)
        {
            var fraValuta = request.FraValuta.ToUpper();
            var availableCurrencies = await GetAvailableCurrencies();

            if (!availableCurrencies.Contains(fraValuta))
                return CreateCurrencyUnavailableResponse(nameof(fraValuta), fraValuta);

            var tilValuta = request.TilValuta.ToUpper();

            if (!availableCurrencies.Contains(tilValuta))
                return CreateCurrencyUnavailableResponse(nameof(tilValuta), tilValuta);

            var isEmptyDate = string.IsNullOrEmpty(request.Dato) || string.IsNullOrWhiteSpace(request.Dato);

            if (fraValuta.Equals(tilValuta))
                return CreateConversionResponse(request, isEmptyDate, request.Belop);

            var result = isEmptyDate ?
                await ConvertCurrencyEmptyDate(tilValuta, fraValuta, request.Belop) :
                await ConvertCurrency(tilValuta, fraValuta, request.Belop, request.Dato);

            return CreateConversionResponse(request, isEmptyDate, result);
        }

        private async Task<decimal> ConvertCurrencyEmptyDate(string tilValuta, string fraValuta, int belop)
        {
            var response = await GetLatestRates(tilValuta, fraValuta);
            var rate = response.rates[tilValuta];

            return belop * rate;
        }

        private async Task<decimal> ConvertCurrency(string to, string from, int amount, string date)
        {
            var response = (await _client.GetAsync(string.Format(_urlConvert, to, from, amount, date))).EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            var convertResponse = JsonSerializer.Deserialize<ConvertResponse>(responseString);

            return convertResponse.result;
        }

        public async Task<LatestRatesResponse> GetLatestRates(string tilValuta, string fraValuta)
        {
            var response = (await _client.GetAsync(string.Format(_urlLatest, tilValuta, fraValuta))).EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<LatestRatesResponse>(responseString); ;
        }

        private ConversionResponse CreateConversionResponse(ConversionRequest request, bool isEmptyDate, decimal result)
        {
            return new ConversionResponse()
            {
                FraValuta = request.FraValuta.ToUpper(),
                TilValuta = request.TilValuta.ToUpper(),
                Belop = request.Belop,
                Dato = isEmptyDate ? DateTime.Today.ToString("yyyy-MM-dd") : request.Dato,
                Result = result
            };
        }

        private ConversionResponse CreateCurrencyUnavailableResponse(string argName, string currencyCode)
        {
            return new ConversionResponse()
            {
                ErrorOccured = true,
                ErrorMessage = $"Invalid {argName}: {currencyCode} is not among available currencies."
            };
        }

        public async Task<List<string>> GetAvailableCurrencies()
        {
            var response = (await _client.GetAsync(_urlSymbols)).EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            var symbolsResponse = JsonSerializer.Deserialize<SymbolsResponse>(responseString);

            return symbolsResponse.symbols.Keys.ToList();
        }
    }
}