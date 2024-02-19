using unwinder.Models.AmadeusApiServiceModels.GetLocationModels;
using unwinder.Services.AmadeusApiService.GetLocation;

namespace unwinder.Services.AmadeusApiService.GetCityIataCode;

/// <summary>
/// Provides the implementation for retrieving IATA code for a city based on a search keyword.
/// </summary>
public class GetCityIataCodeService : IGetCityIataCodeService
{
    private readonly IGetLocationService _getLocationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCityIataCodeService"/> class.
    /// </summary>
    /// <param name="getLocationService">The service used to retrieve location data.</param>
    public GetCityIataCodeService(IGetLocationService getLocationService)
    {
        _getLocationService = getLocationService;
    }

    /// <inheritdoc />
    public async Task<string> GetCityIataCode(string keyword)
    {
        IEnumerable<GetLocationAirportModel> airports;
        try
        {
            airports = await _getLocationService.GetLocation(keyword);
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error retrieving location data: {ex.Message}", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new Exception($"Error processing location data: {ex.Message}", ex);
        }

        if (airports == null || !airports.Any())
        {
            throw new Exception("No airports found for the given location.");
        }

        string cityCode = airports.Select(a => a.CityCode).Distinct().FirstOrDefault();
        if (cityCode == null)
        {
            throw new Exception("City code could not be determined from the available data.");
        }

        return cityCode;
    }
}
