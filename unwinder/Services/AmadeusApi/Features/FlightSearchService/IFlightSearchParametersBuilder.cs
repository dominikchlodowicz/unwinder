namespace unwinder.Services.AmadeusApiService.FlightSearch;

public interface IFlightSearchParametersBuilder
{
    public void BuildDefaultValues();

    public void BuildOriginDestinations();

    public void BuildDateTimeRange();

    public void BuidNumberOfTravelers();
}