using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

namespace unwinder.Services.AmadeusApiService.FlightSearch;

/// <summary>
/// Defines the contract for a service that performs flight offer searches.
/// </summary>
public interface IFlightSearchService
{
    /// <summary>
    /// Searches for flight offer and procesess obtained response.
    /// </summary>
    /// <param name="flightSearchParameters">Parameters for flight search as FlightSearchParameters object.</param>
    /// <returns>Processed flight response as FlightSearchOutputModel object.</returns>
    public Task<FlightSearchOutputModel> FlightSearch(FlightSearchParameters flightSearchParameters);
}