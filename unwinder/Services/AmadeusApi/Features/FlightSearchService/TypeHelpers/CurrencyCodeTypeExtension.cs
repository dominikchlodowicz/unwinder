using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels.TypeHelperModels;

namespace unwinder.Services.AmadeusApiService.FlightSearch.TypeHelpers;

/// <summary>
/// Provides extension methods for currency code validation and conversion.
/// </summary>
public static class CurrencyCodeTypeExtension
{
    /// <summary>
    /// Converts a string value to its corresponding <see cref="CurrencyCodeType"/> enum representation.
    /// </summary>
    /// <param name="value">The currency code as a string.</param>
    /// <returns>The string representation of the <see cref="CurrencyCodeType"/> enum.</returns>
    /// <exception cref="ArgumentException">Thrown if the value does not correspond to any <see cref="CurrencyCodeType"/>.</exception>
    public static string ToCurrencyCodeType(this string value)
    {
        if (Enum.TryParse(value.ToUpperInvariant(), out CurrencyCodeType type))
        {
            return type.ToString();
        }
        throw new ArgumentException($"Invalid currency code type: {value}");
    }
}