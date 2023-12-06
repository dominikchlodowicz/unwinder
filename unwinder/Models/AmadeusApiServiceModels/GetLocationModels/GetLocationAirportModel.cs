using Newtonsoft.Json;

namespace unwinder.Models.AmadeusApiServiceModels.GetLocationModels;

public class GetLocationAirportModel
{
    [JsonProperty("name")]
    public string Name;

    [JsonProperty("iataCode")]
    public string IataCode;

    [JsonProperty("cityName")]
    public string CityName;

    [JsonProperty("countryName")]
    public string CountryName;
}