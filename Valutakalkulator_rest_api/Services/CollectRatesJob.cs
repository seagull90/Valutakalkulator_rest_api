using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Valutakalkulator_rest_api.Models;
using Valutakalkulator_rest_api.Models.Responses;
using Valutakalkulator_rest_api.Repositories;

namespace Valutakalkulator_rest_api.Services
{
    [DisallowConcurrentExecution]
    public class CollectRatesJob : IJob
    {
        private readonly JobConfig _config;
        private readonly IConversionService _conversionService;
        private readonly ICurrencyRepository _currencyRepository;

        public CollectRatesJob(IOptions<JobConfig> config,
            IConversionService conversionService,
            ICurrencyRepository currencyRepository)
        {
            _config = config.Value;
            _conversionService = conversionService;
            _currencyRepository = currencyRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Debug.WriteLine($"Starting {nameof(CollectRatesJob)} execution.");

            var fromCurrencies = _config.FromCurrencies;
            var toCurrencies = _config.ToCurrencies;

            foreach (var from in fromCurrencies)
            {
                //await SaveRatesAsync(from, toCurrencies.Where(x => x != from));
            }
        }

        private async Task SaveRatesAsync(string from, IEnumerable<string> toCurrencies)
        {
            var latestRates = await _conversionService.GetLatestRates(string.Join(",", toCurrencies), from);
            var baseCurrency = CreateBaseCurrency(latestRates);
            await _currencyRepository.SaveAsync(baseCurrency);
        }

        private BaseCurrency CreateBaseCurrency(LatestRatesResponse response)
        {
            return new BaseCurrency
            {
                Code = response.@base,
                Date = DateTime.Parse(response.date), 
                Timestamp = response.timestamp,
                Rates = response.rates.Select(x => new Rate() { Code = x.Key, ExchangeRate = x.Value }).ToList()
            };
        }
    }
}