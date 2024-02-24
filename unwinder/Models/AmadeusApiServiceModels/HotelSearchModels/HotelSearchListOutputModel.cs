namespace unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;

public class HotelSearchAddress
{
    public string CountryCode { get; set; }
}

public class HotelSearchDatum
{
    public string ChainCode { get; set; }
    public string IataCode { get; set; }
    public int DupeId { get; set; }
    public string Name { get; set; }
    public string HotelId { get; set; }
    public HotelSearchGeoCode GeoCode { get; set; }
    public HotelSearchAddress Address { get; set; }
    public HotelSearchDistance Distance { get; set; }
    public DateTime LastUpdate { get; set; }
}

public class HotelSearchDistance
{
    public double Value { get; set; }
    public string Unit { get; set; }
}

public class HotelSearchGeoCode
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class HotelSearchLinks
{
    public string Self { get; set; }
}

public class HotelSearchMeta
{
    public int Count { get; set; }
    public HotelSearchLinks Links { get; set; }
}

public class HotelSearchListOutputModel
{
    public List<HotelSearchDatum> Data { get; set; }
    public HotelSearchMeta Meta { get; set; }
}