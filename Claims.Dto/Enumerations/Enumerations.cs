
namespace Claims.Dto.Enumerations
{
    public class Enumerations
    {
        public enum CoverType
        {
            Yacht = 0,
            PassengerShip = 1,
            ContainerShip = 2,
            BulkCarrier = 3,
            Tanker = 4
        }
        
        public enum ClaimType
        {
            Collision = 0,
            Grounding = 1,
            BadWeather = 2,
            Fire = 3
        }
    }
}
