using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Valutakalkulator_rest_api.Models;

namespace Valutakalkulator_rest_api.Repositories
{
    public interface ICurrencyRepository
    {
        public Task<int> SaveAsync(BaseCurrency response);
        public IEnumerable<BaseCurrency> GetRatesInterval(string fraValuta, DateTime from, DateTime to);
    }
}
