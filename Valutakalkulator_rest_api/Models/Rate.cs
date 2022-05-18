using System;

namespace Valutakalkulator_rest_api.Models
{
    public class Rate
    {
        public BaseCurrency BaseCurrency { get; private set; }

        public int Id { get; private set; }

        public string Code { get; set; }

        public decimal ExchangeRate { get; set; }
    }
}
