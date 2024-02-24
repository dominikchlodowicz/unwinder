using System.Globalization;

namespace unwinder.Services.AmadeusApiService.FlightSearch.TypeHelpers;

public static class DateTimeRangeTypeExtension
{
    /// <summary>
    /// Validate if format of the departureDate and departureTime are in correct format, 
    /// and if the departureDate is not in the past.
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

    /// <summary>
    /// Validates that a given date string is in the correct ISO format (YYYY-MM-DD) and not in the past.
    /// </summary>
    /// <param name="argDate">The date string to validate.</param>
    /// <exception cref="ArgumentException">Thrown if the date string is not in the correct ISO format or represents a date in the past.</exception>
    public static void IsDateInCorrectIsoFormat(string argDate)
    {
        if (!DateTime.TryParse(argDate, out var parsedDate) ||
            !parsedDate.ToString("yyyy-MM-dd").Equals(argDate))
        {
            throw new ArgumentException("Departure date is not in the correct ISO format (YYYY-MM-DD).");
        }

        if (parsedDate.Date < DateTime.Now.Date)
        {
            throw new ArgumentException("Departure DateTime is in the past.");
        }
    }
}

