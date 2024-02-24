using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;

namespace unwinder.Services.AmadeusApiService.HotelSearch;

public class HotelSearchListService : IHotelSearchListService
{
    private IGetToken _getToken;
    private HttpClient _httpClientV1;

    public HotelSearchListService(IHttpClientFactory httpClientFactory, IGetToken getToken)
    {
        _httpClientV1 = httpClientFactory.CreateClient("AmadeusApiV1");
        _getToken = getToken;
    }

    public Task SearchListOfHotels
            (HotelSearchListParametersModel hotelSearchListParametersModel)
    {
        throw new NotImplementedException();
    }
}