using unwinder.Models.AmadeusApiServiceModels.GetLocationModels;

namespace unwinder.Services.AmadeusApiService;

public interface IGetLocationService
{
    public Task<IEnumerable<GetLocationAirportModel>> GetLocation(string query);
}