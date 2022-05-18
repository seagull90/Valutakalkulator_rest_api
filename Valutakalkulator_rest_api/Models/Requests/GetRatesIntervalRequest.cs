using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace Valutakalkulator_rest_api.Models.Requests
{
    public class GetRatesIntervalRequest : BaseRequest
    {
        public string FraDato { get; set; }
        public string TilDato { get; set; }

        public class GetRatesIntervalValidator : BaseRequestValidator<GetRatesIntervalRequest>
        {
            private readonly Regex datoRegex = new Regex(@"^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$");

            public GetRatesIntervalValidator()
            {
                RuleFor(x => x.TilDato)
                    .Custom((dato, context) =>
                    {
                        LessThanCurrentDate(dato, context);
                    });
                RuleFor(x => x.FraDato)
                    .Custom((dato, context) =>
                    {
                        LessThanCurrentDate(dato, context);                        
                    });
            }

            private bool IsDateInvalid(string dato, ValidationContext<GetRatesIntervalRequest> context)
            {
                if (string.IsNullOrEmpty(dato) || string.IsNullOrWhiteSpace(dato))
                {
                    context.AddFailure("Cannot be empty.");

                    return true;
                }

                if (!datoRegex.IsMatch(dato))
                {
                    context.AddFailure("Must have YYYY-MM-DD format f.eks 2018-01-01.");

                    return true;
                }

                return false;
            }

            private void LessThanCurrentDate(string dato, ValidationContext<GetRatesIntervalRequest> context)
            {
                if (IsDateInvalid(dato, context))
                    return;

                var dateTime = DateTime.Parse(dato);

                if (dateTime.Date > DateTime.Today)
                {
                    context.AddFailure("Cannot be greater than the current date.");
                    return;
                }
            }
        }
    }
}