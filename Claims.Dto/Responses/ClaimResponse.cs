using static Claims.Dto.Enumerations.Enumerations;

namespace Claims.Dto.Responses
{
    public class ClaimResponse
    {
        public string Id { get; set; }
        public string CoverId { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public Enumerations.Enumerations.ClaimType Type { get; set; }
        public decimal DamageCost { get; set; }
    }
}
