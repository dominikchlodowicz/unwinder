using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;

public class HotelSearchAddress
{
    [JsonProperty("countryCode")]
    public string CountryCode { get; set; }
}

public class HotelSearchDatum
{
    [JsonProperty("chainCode")]
    public string ChainCode { get; set; }

    [JsonProperty("iataCode")]
    public string IataCode { get; set; }

    [JsonProperty("dupeId")]
    public int DupeId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("hotelId")]
    public string HotelId { get; set; }

    [JsonProperty("geoCode")]
    public HotelSearchGeoCode GeoCode { get; set; }

    [JsonProperty("address")]
    public HotelSearchAddress Address { get; set; }

    [JsonProperty("distance")]
    public HotelSearchDistance Distance { get; set; }

    [JsonProperty("lastUpdate")]
    public DateTime LastUpdate { get; set; }
}

public class HotelSearchDistance
{
    [JsonProperty("value")]
    public double Value { get; set; }

    [JsonProperty("unit")]
    public string Unit { get; set; }
}

public class HotelSearchGeoCode
{
    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("longitude")]
    public double Longitude { get; set; }
}

public class HotelSearchLinks
{
    [JsonProperty("self")]
    public string Self { get; set; }
}

public class HotelSearchMeta
{
    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("links")]
    public HotelSearchLinks Links { get; set; }
}

public class HotelSearchListOutputModel
{
    [JsonProperty("data")]
    public List<HotelSearchDatum> Data { get; set; }

    [JsonProperty("meta")]
    public HotelSearchMeta Meta { get; set; }
}
