using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

namespace unwinder.Services.AmadeusApiService.FlightSearch;

public interface IFlightSearchService
{
    public Task<FlightSearchOutputModel> FlightSearch(FlightSearchParameters flightSearchParameters);

}