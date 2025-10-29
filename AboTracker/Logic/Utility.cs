using System.Text;

namespace AboTracker.Logic;

public class Utility
{
    public static string ToUpperFirst(string str)
    {
        return char.ToUpper(str[0]) + str[1..];
    }
}