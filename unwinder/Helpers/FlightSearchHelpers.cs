using System.Reflection.Metadata.Ecma335;
using Microsoft.VisualBasic;

namespace unwinder.Helpers;

public class FlightSearchHelpers
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
}
