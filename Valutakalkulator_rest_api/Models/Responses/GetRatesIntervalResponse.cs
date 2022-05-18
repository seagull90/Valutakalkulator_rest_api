using System.Collections.Generic;

namespace Valutakalkulator_rest_api.Models.Responses
{
    public class GetRatesIntervalResponse : BaseResponse
    {
        public string Base { get; set; }
        public List<Item> Items { get; set; }
    }
}
