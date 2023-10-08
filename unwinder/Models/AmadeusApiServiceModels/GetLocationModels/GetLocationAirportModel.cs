using Newtonsoft.Json;

namespace unwinder.Models.AmadeusApiServiceModels.GetLocationModels;

public class GetLocationAirportModel
{
    [JsonProperty("name")]
    public string name;

    [JsonProperty("iataCode")]
    public string iataCode;

    [JsonProperty("cityName")]
    public string cityName;
}