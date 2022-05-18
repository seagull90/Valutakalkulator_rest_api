using System;
using System.Collections.Generic;

namespace Valutakalkulator_rest_api.Models
{
    public class BaseCurrency
    {
        public List<Rate> Rates { get; set; }

        public int Id { get; private set; }

        public string Code { get;  set; }

        public DateTime Date { get;  set; }

        public long Timestamp { get;  set; }

    }
}
