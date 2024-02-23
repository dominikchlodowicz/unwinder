using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;

namespace unwinder.Services.AmadeusApiService.HotelSearch;

public class HotelSearchService : IHotelSearchService, IHotelSearchListService
{
    public HotelSearchService()
    {

    }

    public Task SearchListOfHotels
            (HotelSearchListParametersModel hotelSearchListParametersModel)
    {
        throw new NotImplementedException();
    }

    public Task SearchHotel
        (HotelSearchListParametersModel hotelSearchListParametersModel,
            HotelSearchParametersModel hotelSearchParameters)
    {
        throw new NotImplementedException();
    }

}