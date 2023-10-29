using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

namespace unwinder.Services.AmadeusApiService.FlightSearch;

public interface IFlightSearchParametersBuilder
{
    public FlightSearchParametersBuilder BuildNumberOfTravelers(List<string> numberOfTravelers);

    public FlightSearchParametersBuilder BuildDateTimeRange(string departureDate, string departureTime);

    public FlightSearchParametersBuilder BuildOriginDestinations(string originLocationCode, string destinationLocationCode);

    public FlightSearchParametersBuilder BuildCurrencyCode(string currencyCode);

    public FlightSearchParametersBuilder BuildDefaultValues();

    public FlightSearchParameters Build();
}