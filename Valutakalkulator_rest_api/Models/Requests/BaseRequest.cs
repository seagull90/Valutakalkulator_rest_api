using FluentValidation;
using System.Text.RegularExpressions;

namespace Valutakalkulator_rest_api.Models.Requests
{
    public class BaseRequest
    {
        public string FraValuta { get; set; }
        public string TilValuta { get; set; }
        public int Belop { get; set; }
    }
}