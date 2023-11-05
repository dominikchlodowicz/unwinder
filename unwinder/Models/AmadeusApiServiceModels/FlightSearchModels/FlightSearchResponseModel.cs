using Newtonsoft.Json;

namespace unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

// Generated with json to object converter
    public class Aircraft
    {
        public string code { get; set; }

        [JsonProperty("320")]
        public string _320 { get; set; }

        [JsonProperty("744")]
        public string _744 { get; set; }

        [JsonProperty("777")]
        public string _777 { get; set; }
    }

    public class Arrival
    {
        public string iataCode { get; set; }
        public DateTime at { get; set; }
    }

    public class Carriers
    {
        [JsonProperty("6X")]
        public string _6X { get; set; }
    }

    public class Currencies
    {
        public string USD { get; set; }
    }

    public class Datum
    {
        public string type { get; set; }
        public string id { get; set; }
        public string source { get; set; }
        public bool instantTicketingRequired { get; set; }
        public bool nonHomogeneous { get; set; }
        public bool oneWay { get; set; }
        public string lastTicketingDate { get; set; }
        public int numberOfBookableSeats { get; set; }
        public List<ItineraryResponse> itineraries { get; set; }
        public Price price { get; set; }
        public PricingOptions pricingOptions { get; set; }
        public List<string> validatingAirlineCodes { get; set; }
        public List<TravelerPricing> travelerPricings { get; set; }
    }

    public class Departure
    {
        public string iataCode { get; set; }
        public DateTime at { get; set; }
    }

    public class Dictionaries
    {
        public Locations locations { get; set; }
        public Aircraft aircraft { get; set; }
        public Currencies currencies { get; set; }
        public Carriers carriers { get; set; }
    }

    public class EWR
    {
        public string cityCode { get; set; }
        public string countryCode { get; set; }
    }

    public class FareDetailsBySegment
    {
        public string segmentId { get; set; }
        public string cabin { get; set; }
        public string fareBasis { get; set; }
        public string @class { get; set; }
        public IncludedCheckedBags includedCheckedBags { get; set; }
    }

    public class Fee
    {
        public string amount { get; set; }
        public string type { get; set; }
    }

    public class IncludedCheckedBags
    {
        public int quantity { get; set; }
    }

    public class ItineraryResponse
    {
        public string duration { get; set; }
        public List<Segment> segments { get; set; }
    }

    public class JFK
    {
        public string cityCode { get; set; }
        public string countryCode { get; set; }
    }

    public class LHR
    {
        public string cityCode { get; set; }
        public string countryCode { get; set; }
    }

    public class Locations
    {
        public EWR EWR { get; set; }
        public MAD MAD { get; set; }
        public LHR LHR { get; set; }
        public JFK JFK { get; set; }
    }

    public class MAD
    {
        public string cityCode { get; set; }
        public string countryCode { get; set; }
    }

    public class Meta
    {
        public int count { get; set; }
    }

    public class Operating
    {
        public string carrierCode { get; set; }
    }

    public class Price
    {
        public string currency { get; set; }
        public string total { get; set; }
        public string @base { get; set; }
        public List<Fee> fees { get; set; }
        public string grandTotal { get; set; }
    }

    public class PricingOptions
    {
        public List<string> fareType { get; set; }
        public bool includedCheckedBagsOnly { get; set; }
    }

    public class FlightSearchResponseModel
    {
        public Meta meta { get; set; }
        public List<Datum> data { get; set; }
        public Dictionaries dictionaries { get; set; }
    }

    public class Segment
    {
        public Departure departure { get; set; }
        public Arrival arrival { get; set; }
        public string carrierCode { get; set; }
        public string number { get; set; }
        public Aircraft aircraft { get; set; }
        public Operating operating { get; set; }
        
        public string id { get; set; }
        public int numberOfStops { get; set; }
        public bool blacklistedInEU { get; set; }
    }

    public class TravelerPricing
    {
        public string travelerId { get; set; }
        public string fareOption { get; set; }
        public string travelerType { get; set; }
        public Price price { get; set; }
        public List<FareDetailsBySegment> fareDetailsBySegment { get; set; }
    }

