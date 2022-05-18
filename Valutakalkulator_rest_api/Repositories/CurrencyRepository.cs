using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Valutakalkulator_rest_api.Models;

namespace Valutakalkulator_rest_api.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly CurrencyContext _context;

        public CurrencyRepository(CurrencyContext context)
        {
            _context = context;
        }

        public IEnumerable<BaseCurrency> GetRatesInterval(string currencyCode, DateTime from, DateTime to)
        {
            return _context.BaseCurrency.Include(bc => bc.Rates).Where(x => x.Code == currencyCode && x.Date >= from && x.Date <= to).ToList();
        }

        public async Task<int> SaveAsync(BaseCurrency baseCurrency)
        {
            _context.BaseCurrency.Add(baseCurrency);

            try
            {
                int result = await _context.SaveChangesAsync();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}