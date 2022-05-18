using System.Collections.Generic;

namespace Valutakalkulator_rest_api.Models.Responses
{
    public class LatestRatesResponse
    {
        public string @base { get; set; }
        public string date { get; set; }
        public Dictionary<string, decimal> rates { get; set; }
        public long timestamp { get; set; }
    }
}