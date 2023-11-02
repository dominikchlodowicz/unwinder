using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels.HelperModels;

namespace unwinder.Services.AmadeusApiService.FlightSearch.Helpers;

public static class CurrencyCodeTypeExtension
{
    public static string ToCurrencyCodeType(this string value)
    {
        if (Enum.TryParse(value.ToUpperInvariant(), out CurrencyCodeType type))
        {
            return type.ToString();
        }
        throw new ArgumentException($"Invalid currency code type: {value}");
    }
}