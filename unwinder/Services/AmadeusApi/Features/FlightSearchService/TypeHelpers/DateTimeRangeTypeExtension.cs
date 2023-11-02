using System.Globalization;
using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;

namespace unwinder.Services.AmadeusApiService.FlightSearch.Helpers;

public static class DateTimeRangeTypeExtension
{
    /// <summary>
    /// Validate if format of the departureDate and departureTime is in correct format.
    /// </summary>
    /// <param name="departureDate">In ISO 8601 YYYY-MM-DD format.</param>
    /// <param name="departureTime">Local time. hh:mm:ss format, e.g 10:30:00</param>
    /// <exception cref="ArgumentException"></exception>
    public static void DateTimeToCorrectIsoFormat(string departureDate, string departureTime)
    {
        if (!DateTime.TryParse(departureDate, out var parsedDate) ||
            !parsedDate.ToString("yyyy-MM-dd").Equals(departureDate))
        {
            throw new ArgumentException("Departure date is not in the correct ISO format (YYYY-MM-DD).");
        }

        if (!TimeSpan.TryParseExact(departureTime, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out var parsedTime))
        {
            throw new ArgumentException("Departure time is not in the correct ISO format (HH:MM:SS).");
        }
    }
}

