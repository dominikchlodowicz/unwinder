
using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;

namespace unwinder.Services.AmadeusApiService.HotelSearch;

public interface IHotelSearchListService
{
    public Task SearchListOfHotels(HotelSearchListParametersModel hotelSearchListParametersModel);
}