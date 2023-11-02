using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels.HelperModels;

namespace unwinder.Services.AmadeusApiService.FlightSearch.Helpers;

public static class TravelerTypeExtension
{
    public static string ToTravelerType(this string value)
    {
        if (Enum.TryParse(value.ToUpperInvariant(), out TravelerType type))
        {
            return type.ToString();
        }
        throw new ArgumentException($"Invalid traveler type: {value}");
    }
}