namespace unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

public class RootObject
{
    public Meta Meta { get; set; }
    public List<FlightSearchOutputListModel> Data { get; set; }
}

public class Meta
{
    public int Count { get; set; }
    public Links Links { get; set; }
}

public class Links
{
    public string Self { get; set; }
    // ... other properties
}

public class FlightSearchOutputListModel
{
    public FlightSearchOutput FlightSearchOutput { get; set; }
}

public class FlightSearchOutput
{
    public string Type { get; set; }
    public string Id { get; set; }
    public string Source { get; set; }
    public string Destination { get; set; }
    public Price Price { get; set; }
    public List<Segment> Segments { get; set; }
}

public class Price
{
    public string Currency { get; set; }
    public string Total { get; set; }
}

public class Segment
{
    public string Departure { get; set; }
    public string Arrival { get; set; }
}
