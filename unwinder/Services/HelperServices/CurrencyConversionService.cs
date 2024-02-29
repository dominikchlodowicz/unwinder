using System.Globalization;
using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;

namespace unwinder.Services.HelperServices;

/// <summary>
/// Provides services for converting currency values within hotel search results.
/// </summary>
public class CurrencyConversionService : ICurrencyConversionService
{
    /// <summary>
    /// Converts the currency of hotel prices from their original currency to a target currency
    /// specified within the hotel search output model's dictionaries.
    /// </summary>
    /// <param name="hotelSearchOutputData">A reference to the hotel search output model whose currency will be converted.
    /// The method mutates this object directly, updating its converted currency price.</param>
    /// <remarks>
    /// The conversion uses rates found in the <paramref name="hotelSearchOutputData"/>'s dictionaries.
    /// If dictionaries are null or do not contain the necessary conversion rates, the original currency and price are retained.
    /// </remarks>
    public void HotelConvertCurrrency(ref HotelSearchOutputModel hotelSearchOutputData)
    {
        hotelSearchOutputData.ConvertedCurrencyPrice = new ConvertedCurrencyPrice()
        {
        };

        string currency = hotelSearchOutputData.Data[0].Offers[0].Price.Currency;
        int priceValue = (int)Math.Round(decimal.Parse(hotelSearchOutputData.Data[0].Offers[0].Price.Total, CultureInfo.InvariantCulture));

        if (hotelSearchOutputData.Dictionaries != null)
        {
            decimal priceValueDecimal = priceValue;

            decimal conversionRate = decimal.Parse(hotelSearchOutputData.Dictionaries.CurrencyConversionLookupRates[currency].Rate, CultureInfo.InvariantCulture);
            string currencyTarget = hotelSearchOutputData.Dictionaries.CurrencyConversionLookupRates[currency].Target;

            hotelSearchOutputData.ConvertedCurrencyPrice.CurrencyCode = currencyTarget;
            hotelSearchOutputData.ConvertedCurrencyPrice.Value = (int)Math.Round(priceValueDecimal * conversionRate); ;
        }
        else
        {
            hotelSearchOutputData.ConvertedCurrencyPrice.CurrencyCode = currency;
            hotelSearchOutputData.ConvertedCurrencyPrice.Value = priceValue;
        }
    }
}