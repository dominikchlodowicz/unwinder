
using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;

namespace unwinder.Services.AmadeusApiService.HotelSearch;

public interface IHotelSearchListService
{
    public Task<HotelSearchListOutputModel> SearchListOfHotels(HotelSearchListParametersModel hotelSearchListParametersModel);
}