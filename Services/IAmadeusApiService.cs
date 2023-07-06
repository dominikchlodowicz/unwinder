using unwinder.Models;

namespace unwinder.Services;

public interface IAmadeusApiService
{
     Task<string> GetLocation(string query);
     Task<FlightSearchOutputModel> SearchFlights();
}