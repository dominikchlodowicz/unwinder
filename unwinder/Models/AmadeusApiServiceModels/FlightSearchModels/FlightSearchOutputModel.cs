using Newtonsoft.Json;

namespace unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

public class MetaObject
{
    [JsonProperty("count")]
    public int Count { get; set; }
}

public class DepartureOutput
{
    [JsonProperty("iataCode")]
    public string IataCode { get; set; }
}

public class ArrivalOutput
{
    [JsonProperty("iataCode")]
    public string IataCode { get; set; }
}

public class SegmentObject1
{
    [JsonProperty("departure")]
    public DepartureOutput Departure { get; set; }

    [JsonProperty("arrival")]
    public ArrivalOutput Arrival { get; set; }

    [JsonProperty("duration")]
    public string duration { get; set; }

    [JsonProperty("carrierCode")]
    public string carrierCode { get; set; }

    [JsonProperty("number")]
    public string number { get; set; }
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

public class FlightSearchOutputModel
{
    [JsonProperty("meta")]
    public MetaObject Meta { get; set; }

    [JsonProperty("data")]
    public List<FlightOfferData> Data { get; set; }

    public FlightBack FlightBackData { get; set; }
}

public class FlightBack
{
    public string FlightBackDate { get; set; }
}
