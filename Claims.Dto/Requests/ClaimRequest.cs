
using static Claims.Dto.Enumerations.Enumerations;

namespace Claims.Dto.Requests
{
    public class ClaimRequest
    {
        public string CoverId { get; set; }

        public DateTime Created { get; set; }

        public string Name { get; set; }

        public CoverType Type { get; set; }

        public decimal DamageCost { get; set; }
    }
}
