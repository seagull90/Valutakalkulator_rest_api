using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace Valutakalkulator_rest_api.Models.Requests
{
    public class ConversionRequest : BaseRequest
    {
        public string Dato { get; set; }
    }

    public class ConvertRequestValidator : BaseRequestValidator<ConversionRequest>
    {
        private readonly Regex datoRegex = new Regex(@"^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$");

        public ConvertRequestValidator()
        {
            RuleFor(x => x.Dato)
                .Custom((dato, context) =>
                {
                    if (string.IsNullOrEmpty(dato) || string.IsNullOrWhiteSpace(dato))
                        return;

                    if (!datoRegex.IsMatch(dato))
                    {
                        context.AddFailure("Must have YYYY-MM-DD format f.eks 2018-01-01.");
                        return;
                    }

                    var dateTime = DateTime.Parse(dato);

                    if (dateTime.Date > DateTime.Today)
                    {
                        context.AddFailure("Cannot be greater than the current date.");
                        return;
                    }
                });
        }
    }
}