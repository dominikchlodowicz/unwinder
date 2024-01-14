using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels.TypeHelperModels;

namespace unwinder.Services.AmadeusApiService.FlightSearch.TypeHelpers;

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