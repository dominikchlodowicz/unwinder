using Newtonsoft.Json;
using unwinder.Services.AmadeusApiService.HotelSearch;
using unwinder.Services.HelperServices;

namespace unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;

public class Average
{
    [JsonProperty("base")]
    public string Base { get; set; }
}

public class Change
{
    [JsonProperty("startDate")]
    public string StartDate { get; set; }

    [JsonProperty("endDate")]
    public string EndDate { get; set; }

    [JsonProperty("base")]
    public string Base { get; set; }
}

public class Datum
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("hotel")]
    public Hotel Hotel { get; set; }

    [JsonProperty("available")]
    public bool Available { get; set; }

    [JsonProperty("offers")]
    public List<Offer> Offers { get; set; }

    [JsonProperty("self")]
    public string Self { get; set; }
}

public class Hotel
{
    [JsonProperty("hotelId")]
    public string HotelId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("longitude")]
    public double Longitude { get; set; }
}

public class Offer
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("checkInDate")]
    public string CheckInDate { get; set; }

    [JsonProperty("checkOutDate")]
    public string CheckOutDate { get; set; }

    [JsonProperty("room")]
    public Room Room { get; set; }

    [JsonProperty("price")]
    public Price Price { get; set; }
}

public class Price
{
    [JsonProperty("currency")]
    public string Currency { get; set; }

    [JsonProperty("total")]
    public string Total { get; set; }
}

public class Room
{
    [JsonProperty("typeEstimated")]
    public TypeEstimated TypeEstimated { get; set; }
}

public class HotelSearchOutputModel
{
    [JsonProperty("data")]
    public List<Datum> Data { get; set; }

    [JsonProperty("dictionaries")]
    public Dictionaries Dictionaries { get; set; }

    public ConvertedCurrencyPrice ConvertedCurrencyPrice { get; set; }
}

public class Dictionaries
{
    [JsonProperty("currencyConversionLookupRates")]
    public Dictionary<string, CurrencyConversion> CurrencyConversionLookupRates { get; set; }
}

public class CurrencyConversion
{
    [JsonProperty("rate")]
    public string Rate { get; set; }

    [JsonProperty("target")]
    public string Target { get; set; }

    [JsonProperty("targetDecimalPlaces")]
    public int TargetDecimalPlaces { get; set; }
}

public class TypeEstimated
{
    [JsonProperty("category")]
    public string Category { get; set; }
}

/// <summary>
///    Custom class representing converted price in <see cref="HotelSearchService"/> using <see cref="CurrencyConversionService"/>.
/// </summary>
public class ConvertedCurrencyPrice
{
    public string CurrencyCode { get; set; }
    public int Value { get; set; }
}