using System.Text;

namespace AboTracker.Logic;

public abstract class Utility
{
    public static void ExportToCsv()
    {
        
    }
    
    public static string ToUpperFirst(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }
        
        return char.ToUpper(str[0]) + str.Substring(1);
    }
}