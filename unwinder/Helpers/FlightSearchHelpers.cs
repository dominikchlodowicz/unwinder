namespace unwinder.Helpers;

public static class FlightSearchHelpers
{
    public static List<string> RepeatString(string input, int count)
    {
        var stringList = new List<string>();
        for (int i = 0; i < count; i++)
        {
            stringList.Add(input);
        }
        return stringList;
    }

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
