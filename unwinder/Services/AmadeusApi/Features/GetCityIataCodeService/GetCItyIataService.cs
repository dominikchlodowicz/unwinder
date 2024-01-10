using unwinder.Services.AmadeusApiService.GetLocation;

namespace unwinder.Services.AmadeusApiService.GetCityIataCode;

public class GetCityIataCodeService : IGetCityIataCodeService
{
    private readonly IGetLocationService _getLocationService;

    public GetCityIataCodeService(IGetLocationService getLocationService)
    {
        _getLocationService = getLocationService;
    }

    public async Task<string> GetCityIataCode(string keyword)
    {
        var airports = await _getLocationService.GetLocation(keyword);

        if (airports == null || !airports.Any())
        {
            throw new Exception("No airports found for the given location.");
        }

        string cityCode = airports.Select(a => a.CityCode).Distinct().ToList()[0];

        return cityCode;
    }

}