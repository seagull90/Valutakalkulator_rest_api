namespace Valutakalkulator_rest_api.Models.Responses
{
    public class ConversionResponse : BaseResponse
    {
        public string FraValuta { get; set; }
        public string TilValuta { get; set; }
        public int Belop { get; set; }
        public string Dato { get; set; }
        public decimal Result { get; set; }
    }
}