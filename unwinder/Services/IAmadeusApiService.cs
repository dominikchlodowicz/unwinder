using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using unwinder.Models.AmadeusApiServiceModels.GetLocationModels;

namespace unwinder.Services;

public interface IAmadeusApiService
{
     Task<IEnumerable<GetLocationAirportModel>> GetLocation(string query);
     Task<FlightSearchOutputModel> FlightSearch(FlightSearchParameters flightSearchParameters);
}