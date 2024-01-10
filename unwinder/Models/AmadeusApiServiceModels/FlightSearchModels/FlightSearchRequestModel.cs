using Newtonsoft.Json;

namespace unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

public class FlightSearchRequest
{
    [JsonProperty("where")]
    public string Where { get; set; }
    [JsonProperty("origin")]
    public string Origin { get; set; }
    [JsonProperty("when")]
    public string When { get; set; }
    [JsonProperty("back")]
    public string Back { get; set; }
    [JsonProperty("numberOfPassengers")]
    public int NumberOfPassengers { get; set; }
}