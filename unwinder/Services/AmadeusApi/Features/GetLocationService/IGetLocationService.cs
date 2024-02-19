using unwinder.Models.AmadeusApiServiceModels.GetLocationModels;

namespace unwinder.Services.AmadeusApiService.GetLocation;

/// <summary>
/// Defines the contract for a service to retrieve location information based on a query.
/// </summary>
public interface IGetLocationService
{
    /// <summary>
    /// Retrieves a collection of airport models based on the provided search query.
    /// </summary>
    /// <param name="query">The search query used to find locations, such as airport names or city names.</param>
    /// <returns>A collection of <see cref="GetLocationAirportModel"/> instances matching the search query.</returns>
    public Task<IEnumerable<GetLocationAirportModel>> GetLocation(string query);
}