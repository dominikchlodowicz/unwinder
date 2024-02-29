using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;

namespace unwinder.Services.AmadeusApiService.HotelSearch;

public interface IHotelSearchParametersBuilder
{
    public HotelSearchParametersBuilder BuildHotelIds(HotelSearchListOutputModel hotelSearchListOutput);
    public HotelSearchParametersBuilder BuildNumberOfAdults(int numberOfAdults);
    public HotelSearchParametersBuilder BuildInOutDates(string checkInDate, string checkOutDate);
    public HotelSearchParametersBuilder BuildDefaultValues();
    public HotelSearchParametersModel Build();
}