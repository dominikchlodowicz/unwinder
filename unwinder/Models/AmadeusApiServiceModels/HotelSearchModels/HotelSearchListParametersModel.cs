using Newtonsoft.Json;

namespace unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;

public class HotelSearchListParametersModel
{
    [JsonProperty("cityCode")]
    public string CityCode { get; set; }

    [JsonProperty("radius")]
    public int Radius { get; set; } = 5;
}