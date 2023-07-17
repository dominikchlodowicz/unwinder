
namespace unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
public class FlightSearchParameters
{
    public string OriginLocationCode { get; set; }
    public string DestinationLocationCode { get; set; }
    public DateTime DepartureDate { get; set; }
    public int Adults { get; set; }
    public int Max { get; set; }
}