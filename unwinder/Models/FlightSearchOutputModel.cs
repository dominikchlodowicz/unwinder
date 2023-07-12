using Newtonsoft.Json;

namespace unwinder.Models;

public class FlightSearchOutputModel
{
    public Meta meta { get; set; }
    public Data[] data { get; set; }
}

public class Meta
{
    public int count { get; set; }
}

public class Data
{
    public bool oneWay { get; set; }
    public Itineraries[] itineraries { get; set; }
    public Price price { get; set; }

}

public class Itineraries
{
    public string duration { get; set; }
    public Segments[] segments { get; set; }
}

public class Segments
{
    public Departure departure { get; set; }
    public Arrival arrival { get; set; }
    public string carrierCode { get; set; }
    public string duration { get; set; }
    public int numberOfStops { get; set; }
}

public class Departure
{
    public string iataCode { get; set; }
    public string at { get; set; }
    public string terminal { get; set; }
}

public class Arrival
{
    public string iataCode { get; set; }
    public string at { get; set; }
    public string terminal { get; set; }
}


public class Price
{
    public string currency { get; set; }
    public string total { get; set; }
}

