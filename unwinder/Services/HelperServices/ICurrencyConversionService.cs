using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;

namespace unwinder.Services.HelperServices;

public interface ICurrencyConversionService
{
    void HotelConvertCurrrency(ref HotelSearchOutputModel hotelSearchOutputData);
}

