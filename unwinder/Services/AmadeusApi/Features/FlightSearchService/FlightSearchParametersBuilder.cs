using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

using unwinder.Services.AmadeusApiService.FlightSearch.TypeHelpers;

namespace unwinder.Services.AmadeusApiService.FlightSearch;

public class FlightSearchParametersBuilder : IFlightSearchParametersBuilder
{
    private FlightSearchParameters _parameters = new FlightSearchParameters
    {
        Travelers = new List<Traveler>(),
        OriginDestinations = new List<OriginDestination>(),
        CurrencyCode = string.Empty, // Assuming a default empty value
        SearchCriteria = new SearchCriteria
        {
            FlightFilters = new FlightFilters
            {
                CabinRestrictions = new List<CabinRestriction>()
            }
        },
        Sources = new List<string>()
    };

    private string defaultCabin = "ECONOMY";
    private string defaultCoverage = "MOST_SEGMENTS";
    // number of destinations and their id's
    private List<string> OriginDestinationIds = new List<string> { "1" };
    private int defaultNumberOfFlightOffers = 5;

    private List<string> defaultDataSource = new List<string> { "GDS" };

    private DepartureDateTimeRange _departureDateTimeRange;

    /// <summary>
    /// Builds the number of travelers for the flight search parameters.
    /// </summary>
    /// <param name="numberOfTravelers">A list of strings where each string represents a traveler type. This list cannot be null or empty.</param>
    /// <returns>The instance of <see cref="FlightSearchParametersBuilder"/> for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="numberOfTravelers"/> parameter is null or empty.</exception>
    public FlightSearchParametersBuilder BuildNumberOfTravelers(List<string> numberOfTravelers)
    {
        if (numberOfTravelers.Count == 0 || numberOfTravelers == null)
        {
            throw new ArgumentNullException("numberOfTravelers cannot be empty");
        }

        List<string> typeVerifiedNumberOfTravelers = numberOfTravelers.Select(t => t.ToTravelerType()).ToList();

        for (int traveler_id = 0; traveler_id < typeVerifiedNumberOfTravelers.Count; traveler_id++)
        {
            _parameters.Travelers.Add(
                new Traveler
                {
                    Id = traveler_id.ToString(),
                    TravelerType = typeVerifiedNumberOfTravelers[traveler_id]
                }
            );
        }

        return this;
    }

    /// <summary>
    ///     Sets DepratureDateTimeRange for OriginDestinations object.
    /// </summary>
    /// <param name="departureDate">In ISO 8601 YYYY-MM-DD format.</param>
    /// <param name="departureTime">Local time. hh:mm:ss format, e.g 10:30:00</param>
    /// <returns></returns>
    public FlightSearchParametersBuilder BuildDateTimeRange(string departureDate, string departureTime)
    {
        DateTimeRangeTypeExtension.DateTimeToCorrectIsoFormat(departureDate, departureTime);

        _departureDateTimeRange = new DepartureDateTimeRange
        {
            Date = departureDate,
            Time = departureTime,
        };

        return this;
    }

    /// <summary>
    ///    
    /// </summary>
    /// <param name="originLocationCode">Locaiton aiport IATA string</param>
    /// <param name="destinationLocationCode">Locaiton aiport IATA string</param>
    /// <exception cref="InvalidOperationException"></exception>
    public FlightSearchParametersBuilder BuildOriginDestinations(string originLocationCode, string destinationLocationCode)
    {
        if (_departureDateTimeRange == null)
        {
            throw new InvalidOperationException("The departure date and time must be set before setting origin and destination.");
        }

        _parameters.OriginDestinations.Add(new OriginDestination
        {
            // only one origin destination
            Id = "1",
            OriginLocationCode = originLocationCode,
            DestinationLocationCode = destinationLocationCode,
            DepartureDateTimeRange = _departureDateTimeRange
        });

        return this;
    }

    /// <summary>
    ///     Adds currency code to request
    /// </summary>
    /// <param name="currencyCode">Currently allowed currencies are defined in CurrencyCodeTypeExtension</param>
    public FlightSearchParametersBuilder BuildCurrencyCode(string currencyCode)
    {
        string typeVerifiedCurrencyCode = currencyCode.ToCurrencyCodeType();

        _parameters.CurrencyCode = typeVerifiedCurrencyCode;

        return this;
    }

    /// <summary>
    /// Builds the default values for the flight search parameters including cabin restrictions, maximum flight offers, and data source.
    /// </summary>
    /// <returns>The instance of FlightSearchParametersBuilder for method chaining.</returns>
    public FlightSearchParametersBuilder BuildDefaultValues()
    {
        _parameters.SearchCriteria.FlightFilters.CabinRestrictions.Add(new CabinRestriction
        {
            Cabin = defaultCabin,
            Coverage = defaultCoverage,
            OriginDestinationIds = OriginDestinationIds
        });

        _parameters.SearchCriteria.MaxFlightOffers = defaultNumberOfFlightOffers;

        _parameters.Sources = defaultDataSource;

        return this;
    }

    /// <summary>
    /// Finalizes the construction of the FlightSearchParameters and returns the constructed object.
    /// </summary>
    /// <returns>The fully constructed FlightSearchParameters object.</returns>
    public FlightSearchParameters Build()
    {
        return _parameters;
    }
}