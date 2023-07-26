using Newtonsoft.Json;

namespace unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

public class FlightSearchParameters
{
    [JsonProperty("currencyCode")]
    public string CurrencyCode { get; set; }

    [JsonProperty("originDestinations")]
    public List<OriginDestination> OriginDestinations { get; set; }

    [JsonProperty("travelers")]
    public List<Traveler> Travelers { get; set; }

    [JsonProperty("sources")]
    public List<string> Sources { get; set; }

    [JsonProperty("searchCriteria")]
    public SearchCriteria SearchCriteria { get; set; }
}

public class OriginDestination
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("originLocationCode")]
    public string OriginLocationCode { get; set; }

    [JsonProperty("destinationLocationCode")]
    public string DestinationLocationCode { get; set; }

    [JsonProperty("departureDateTimeRange")]
    public DepartureDateTimeRange DepartureDateTimeRange { get; set; }
}

public class DepartureDateTimeRange
{
    [JsonProperty("date")]
    public string Date { get; set; }

    [JsonProperty("time")]
    public string Time { get; set; }
}

public class Traveler
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("travelerType")]
    public string TravelerType { get; set; }
}

public class SearchCriteria
{
    [JsonProperty("maxFlightOffers")]
    public int MaxFlightOffers { get; set; }

    [JsonProperty("flightFilters")]
    public FlightFilters FlightFilters { get; set; }
}

public class FlightFilters
{
    [JsonProperty("cabinRestrictions")]
    public List<CabinRestriction> CabinRestrictions { get; set; }
}

public class CabinRestriction
{
    [JsonProperty("cabin")]
    public string Cabin { get; set; }

    [JsonProperty("coverage")]
    public string Coverage { get; set; }

    [JsonProperty("originDestinationIds")]
    public List<string> OriginDestinationIds { get; set; }
}
