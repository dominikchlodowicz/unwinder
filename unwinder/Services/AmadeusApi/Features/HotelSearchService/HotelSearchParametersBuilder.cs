using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;
using unwinder.Services.AmadeusApiService.FlightSearch.TypeHelpers;

namespace unwinder.Services.AmadeusApiService.HotelSearch;

public class HotelSearchParametersBuilder : IHotelSearchParametersBuilder
{
    private HotelSearchParametersModel _model = new HotelSearchParametersModel();

    private string defaultCurrency = "EUR";

    private int hotelIdCountLimit = 30;

    public HotelSearchParametersBuilder BuildHotelIds(HotelSearchListOutputModel hotelSearchListOutput)
    {
        if (hotelSearchListOutput.Data.Count == 0)
        {
            throw new ArgumentNullException("HotelListSearch output is empty");
        }

        List<string> hotelIds = new List<string>();
        int counter = 0;

        foreach (HotelSearchDatum data in hotelSearchListOutput.Data)
        {
            if (counter >= hotelIdCountLimit) break; // Stop after adding five IDs

            hotelIds.Add(data.HotelId);
            counter++;
        }

        _model.HotelIds = hotelIds;
        return this;
    }

    public HotelSearchParametersBuilder BuildNumberOfAdults(int numberOfAdults)
    {
        if (numberOfAdults == 0)
        {
            throw new ArgumentNullException("numberOfAdults cannot be empty");
        }

        _model.Adults = numberOfAdults;
        return this;
    }

    public HotelSearchParametersBuilder BuildInOutDates(string checkInDate, string checkOutDate)
    {
        DateTimeRangeTypeExtension.IsDateInCorrectIsoFormat(checkInDate);
        DateTimeRangeTypeExtension.IsDateInCorrectIsoFormat(checkOutDate);

        _model.CheckInDate = checkInDate;
        _model.CheckOutDate = checkOutDate;
        return this;
    }

    public HotelSearchParametersBuilder BuildDefaultValues()
    {
        _model.Currency = defaultCurrency;
        return this;
    }

    public HotelSearchParametersModel Build()
    {
        return _model;
    }
}