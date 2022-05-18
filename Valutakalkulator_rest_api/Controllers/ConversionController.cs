using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Valutakalkulator_rest_api.Models.Requests;
using Valutakalkulator_rest_api.Models.Responses;
using Valutakalkulator_rest_api.Services;

namespace Valutakalkulator_rest_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly IConversionService _conversionService;

        public ConversionController(IConversionService conversionService)
        {
            _conversionService = conversionService;
        }

        [HttpPost("GetRatesInterval")]
        public ActionResult GetRates([FromBody] GetRatesIntervalRequest request)
        {
            string result;

            try
            {
                var response = _conversionService.GetRatesInterval(request);
                result = JsonSerializer.Serialize(response);

                if (response.ErrorOccured)
                    return StatusCode(400, result);
            }
            catch (Exception ex)
            {
                result = JsonSerializer.Serialize(new ConversionResponse() { ErrorOccured = true, ErrorMessage = ex.Message });

                return StatusCode(500, result);
            }

            return Ok(result);
        }

        [HttpPost("RunConversion")]
        public async Task<ActionResult> RunConversion([FromBody] ConversionRequest request)
        {
            string result;

            try
            {
                var response = await _conversionService.RunConversion(request);
                result = JsonSerializer.Serialize(response);

                if (response.ErrorOccured)
                    return StatusCode(400, result);
            }
            catch (Exception ex)
            {
                result = JsonSerializer.Serialize(new ConversionResponse() { ErrorOccured = true, ErrorMessage = ex.Message });

                return StatusCode(500, result);
            }

            return Ok(result);
        }
    }
}