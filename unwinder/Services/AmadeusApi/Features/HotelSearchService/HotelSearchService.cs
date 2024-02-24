using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;

namespace unwinder.Services.AmadeusApiService.HotelSearch;

public class HotelSearchService : IHotelSearchService
{
    private IGetToken _getToken;
    private HttpClient _httpClientV3;

    public HotelSearchService(IHttpClientFactory httpClientFactory, IGetToken getToken)
    {
        _httpClientV3 = httpClientFactory.CreateClient("AmadeusApiV3");
        _getToken = getToken;
    }

    public Task SearchHotel
        (HotelSearchListParametersModel hotelSearchListParametersModel,
            HotelSearchParametersModel hotelSearchParameters)
    {
        throw new NotImplementedException();
    }

}