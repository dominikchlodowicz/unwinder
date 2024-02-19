using System.Globalization;

namespace unwinder.Services.AmadeusApiService.FlightSearch.TypeHelpers
;
/// <summary>
/// Provides extension methods for validating date and time formats.
/// </summary>
public static class DateTimeRangeTypeExtension
{
    /// <summary>
    /// Validates if the format of the departureDate and departureTime are correct according to ISO 8601 and checks if the departureDate is not in the past.
    /// </summary>
    /// <param name="departureDate">The departure date in ISO 8601 format (YYYY-MM-DD).</param>
    /// <param name="departureTime">The local departure time in 24-hour format (HH:MM:SS).</param>
    /// <exception cref="ArgumentException">Thrown if the departure date or time are not in the correct format or if the departure DateTime is in the past.</exception>
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

        //Past check
        var fullDateTimeString = $"{departureDate}T{departureTime}";
        if (!DateTime.TryParseExact(fullDateTimeString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fullDateTime))
        {
            throw new ArgumentException("Combined Departure DateTime is not in the correct format.");
        }

        if (fullDateTime < DateTime.Now)
        {
            throw new ArgumentException("Departure DateTime is in the past.");
        }
    }
}

