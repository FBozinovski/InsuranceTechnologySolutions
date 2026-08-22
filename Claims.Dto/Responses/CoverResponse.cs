using static Claims.Dto.Enumerations.Enumerations;

namespace Claims.Dto.Responses
{
    public class CoverResponse
    {
        public string Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CoverType Type { get; set; }
        public decimal Premium { get; set; }
    }
}
