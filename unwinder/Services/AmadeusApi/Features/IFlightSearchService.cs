using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

namespace unwinder.Services.AmadeusApiService;

public interface IFlightSearchService
{
    public Task<FlightSearchOutputModel> FlightSearch(FlightSearchParameters flightSearchParameters);

}