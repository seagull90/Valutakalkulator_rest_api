using System;
using System.Collections.Generic;

namespace Valutakalkulator_rest_api.Models.Responses
{
    public class Item
    {
        public string Date { get; set; }
        public long Timestamp { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}