using FluentValidation;
using System.Text.RegularExpressions;

namespace Valutakalkulator_rest_api.Models.Requests
{
    public class BaseRequestValidator<T> : AbstractValidator<T> where T : BaseRequest
    {
        private const string message = "Only letters allowed.";
        private readonly Regex valutaRegex = new Regex("^[a-zA-Z]*$");

        public BaseRequestValidator()
        {
            RuleFor(x => x.FraValuta).NotEmpty().Length(exactLength: 3).Matches(valutaRegex).WithMessage(message);
            RuleFor(x => x.TilValuta).NotEmpty().Length(exactLength: 3).Matches(valutaRegex).WithMessage(message);
            RuleFor(x => x.Belop).GreaterThan(0);
        }
    }
}