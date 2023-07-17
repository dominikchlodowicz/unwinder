using System.ComponentModel.DataAnnotations;

namespace unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

public class FlightSearchModel
{
    public string OriginLocationCode { get; set; }
    public string DestinationLocationCode { get; set; }
    [DataType(DataType.Date)]
    public DateTime departureDate { get; set; }
    public int Adults { get; set; }
    public int Max { get; set; } = 5;
}


