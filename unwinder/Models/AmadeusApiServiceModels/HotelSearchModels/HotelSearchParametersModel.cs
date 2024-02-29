using Newtonsoft.Json;

namespace unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;

public class HotelSearchParametersModel
{
    [JsonProperty("hotelIds")]
    public List<string> HotelIds { get; set; }

    [JsonProperty("adults")]
    public int Adults { get; set; }

    [JsonProperty("checkInDate")]
    public string CheckInDate { get; set; }

    [JsonProperty("checkOutDate")]
    public string CheckOutDate { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; }
}