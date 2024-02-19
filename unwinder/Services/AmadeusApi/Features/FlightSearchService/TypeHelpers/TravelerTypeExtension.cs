using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels.TypeHelperModels;

namespace unwinder.Services.AmadeusApiService.FlightSearch.TypeHelpers;

/// <summary>
/// Provides extension methods for traveler type validation and conversion.
/// </summary>
public static class TravelerTypeExtension
{
    /// <summary>
    /// Converts a string value to its corresponding <see cref="TravelerType"/> enum representation.
    /// </summary>
    /// <param name="value">The traveler type as a string.</param>
    /// <returns>The string representation of the <see cref="TravelerType"/> enum.</returns>
    /// <exception cref="ArgumentException">Thrown if the value does not correspond to any <see cref="TravelerType"/>.</exception>
    public static string ToTravelerType(this string value)
    {
        if (Enum.TryParse(value.ToUpperInvariant(), out TravelerType type))
        {
            return type.ToString();
        }
        throw new ArgumentException($"Invalid traveler type: {value}");
    }
}