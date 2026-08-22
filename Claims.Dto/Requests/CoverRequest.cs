using static Claims.Dto.Enumerations.Enumerations;

namespace Claims.Dto.Requests
{
    public class CoverRequest
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public CoverType Type { get; set; }
    }
}
