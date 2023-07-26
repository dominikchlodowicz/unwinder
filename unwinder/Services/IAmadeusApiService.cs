using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

namespace unwinder.Services;

public interface IAmadeusApiService
{
     Task<string> GetLocation(string query);
     Task<FlightSearchOutputModel> FlightSearch(FlightSearchParameters flightSearchParameters);
}