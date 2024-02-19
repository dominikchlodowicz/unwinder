namespace unwinder.Helpers;

/// <summary>
/// Provides utility methods to assist with flight search operations.
/// </summary>
public static class FlightSearchHelpers
{
    /// <summary>
    /// Creates a list containing a specified string repeated a specified number of times.
    /// </summary>
    /// <param name="input">The string to repeat.</param>
    /// <param name="count">The number of times to repeat the string.</param>
    /// <returns>A list of strings where each element is the input string repeated count times.</returns>
    public static List<string> RepeatString(string input, int count)
    {
        var stringList = new List<string>();
        for (int i = 0; i < count; i++)
        {
            stringList.Add(input);
        }
        return stringList;
    }

    /// <summary>
    /// Converts an ISO 8601 date string to a date in "yyyy-MM-dd" format.
    /// </summary>
    /// <param name="isoString">The ISO 8601 date string to convert.</param>
    /// <returns>The date portion of the ISO string in "yyyy-MM-dd" format.</returns>
    /// <exception cref="ArgumentException">Thrown if the input string is not a valid ISO 8601 date.</exception>
    public static string ConvertIsoDateStringToDate(string isoString)
    {
        DateTime parsedDate;
        bool success = DateTime.TryParse(isoString, out parsedDate);

        if (!success)
        {
            throw new ArgumentException("Passed string argument is not a valid ISO date.");
        }

        return parsedDate.Date.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// Converts an ISO 8601 date and time string to a time in "HH:mm:ss" format.
    /// </summary>
    /// <param name="isoString">The ISO 8601 date and time string to convert.</param>
    /// <returns>The time portion of the ISO string in "HH:mm:ss" format.</returns>
    /// <exception cref="ArgumentException">Thrown if the input string is not a valid ISO 8601 date and time.</exception>
    public static string ConvertIsoDateStringToTime(string isoString)
    {
        DateTime parsedDateTime;
        bool success = DateTime.TryParse(isoString, out parsedDateTime);

        if (!success)
        {
            throw new ArgumentException("Passed string argument is not a valid ISO date.");
        }

        return parsedDateTime.ToString("HH:mm:ss");
    }
}
