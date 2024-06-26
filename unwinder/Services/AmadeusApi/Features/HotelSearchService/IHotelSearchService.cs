using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;

namespace unwinder.Services.AmadeusApiService.HotelSearch;

public interface IHotelSearchService
{
    public Task<HotelSearchOutputModel> SearchHotel(HotelSearchParametersModel hotelSearchParameters);
}