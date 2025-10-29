using System.Text;

namespace AboTracker.Logic;

public abstract class Utility
{
    public static void ExportToCsv()
    {
        
    }
    
    public static string ToUpperFirst(string str)
    {
        return char.ToUpper(str[0]) + str[1..];
    }
}