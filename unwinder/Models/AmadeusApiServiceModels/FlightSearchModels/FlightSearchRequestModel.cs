namespace unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

public class FlightSearchRequest
{
    public string Where { get; set; }
    public string Origin { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int NumberOfPassengers { get; set; }
}