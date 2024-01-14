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
        DateTime.TryParse(isoString, out parsedDate);

        return parsedDate.Date.ToString("yyyy-MM-dd");
    }
}
