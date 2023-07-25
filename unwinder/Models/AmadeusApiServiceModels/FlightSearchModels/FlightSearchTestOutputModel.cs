using Newtonsoft.Json;

namespace unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

public class MetaObject
{
    [JsonProperty("count")]
    public int Count { get; set; }
}

public class Departure
{
    [JsonProperty("iataCode")]
    public string IataCode { get; set; }
}

public class Arrival
{
    [JsonProperty("iataCode")]
    public string IataCode { get; set; }
}

public class SegmentObject1
{
    [JsonProperty("departure")]
    public Departure Departure { get; set; }

    [JsonProperty("arrival")]
    public Arrival Arrival { get; set; }
}

public class Itinerary
{
    [JsonProperty("duration")]
    public string Duration { get; set; }

    [JsonProperty("segments")]
    public List<SegmentObject1> Segments { get; set; }
}

public class PriceObject
{
    [JsonProperty("total")]
    public string Total { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; }
}

public class FlightOfferData
{
    [JsonProperty("itineraries")]
    public List<Itinerary> Itineraries { get; set; }

    [JsonProperty("price")]
    public PriceObject Price { get; set; }
}

public class FlightOfferResponse
{
    [JsonProperty("meta")]
    public MetaObject Meta { get; set; }

    [JsonProperty("data")]
    public List<FlightOfferData> Data { get; set; }
}
