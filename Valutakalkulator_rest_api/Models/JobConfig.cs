using System.Collections.Generic;

namespace Valutakalkulator_rest_api.Models
{
    public class JobConfig
    {
        public string Schedule { get; set; }
        public IEnumerable<string> FromCurrencies { get; set; }
        public IEnumerable<string> ToCurrencies { get; set; }

    }
}
