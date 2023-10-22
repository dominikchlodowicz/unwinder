using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

namespace unwinder.Services.AmadeusApiService.FlightSearch;

public class FlightSearchParametersBuilder : IFlightSearchParametersBuilder
{
    private FlightSearchParameters _parameters = new FlightSearchParameters();

    private string defaultTravelerType = "ADULT";
    private string defaultTimeWindow = "12H";
    private string defaultCabin = "ECONOMY";
    private string defaultCoverage = "MOST_SEGMENTS";
    // number of destinations and their id's
    private List<string> OriginDestinationIds = new List<string> { "1" };
    private int defaultNumberOfFlightOffers = 5;

    private DepartureDateTimeRange _departureDateTimeRange;

    // TODO: Add for loop creating travelers if more than 1
    public void BuildNumberOfTravelers(int numberOfTravelers)
    {
        _parameters.Travelers.Add(
            new Traveler{
                Id = numberOfTravelers.ToString(),
                TravelerType = defaultTravelerType
            }
        );
    }

    public DepartureDateTimeRange BuildDateTimeRange(string departureDate, string departureTime)
    {
        _departureDateTimeRange = new DepartureDateTimeRange{
            Date = departureDate,
            Time = departureTime,
            TimeWindow = defaultTimeWindow
        };

        return _departureDateTimeRange;
    }

    public void BuildOriginDestinations(string originLocationCode, string destinationLocationCode, DepartureDateTimeRange departureDateTimeRange)
    {
        _parameters.OriginDestinations.Add(new OriginDestination{
            // only one origin destination
            Id = "1",
            OriginLocationCode = originLocationCode,
            DestinationLocationCode = destinationLocationCode,
            DepartureDateTimeRange = departureDateTimeRange
        });
    }

    // TODO: Add all default values from FlightSearcgParametersModels
    public void BuildDefaultValues()
    {
        _parameters.SearchCriteria.FlightFilters.CabinRestrictions.Add(new CabinRestriction{
            Cabin = defaultCabin,
            Coverage = defaultCoverage,
            OriginDestinationIds = OriginDestinationIds
        });

        _parameters.SearchCriteria.MaxFlightOffers = defaultNumberOfFlightOffers;

    }
}