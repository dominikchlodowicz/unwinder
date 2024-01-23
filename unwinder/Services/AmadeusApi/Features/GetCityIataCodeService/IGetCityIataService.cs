namespace unwinder.Services.AmadeusApiService.GetCityIataCode;

public interface IGetCityIataCodeService
{
    public Task<string> GetCityIataCode(string keyword);
}