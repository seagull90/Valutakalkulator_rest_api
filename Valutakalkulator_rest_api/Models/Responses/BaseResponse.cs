using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Valutakalkulator_rest_api.Models.Responses
{
    public class BaseResponse
    {
        public bool ErrorOccured { get; set; }
        public string ErrorMessage { get; set; }
    }
}
