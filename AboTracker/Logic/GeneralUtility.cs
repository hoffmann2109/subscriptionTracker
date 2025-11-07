using System.Globalization;
    
namespace AboTracker.Logic;

public abstract class GeneralUtility
{
    public static void ExportToCsv()
    {
        // TODO: Implement
    }
    
    public static string ToUpperFirst(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }
        
        return char.ToUpper(str[0]) + str[1..];
    }

    public static string GetCurrencySymbol()
    {
        var region = RegionInfo.CurrentRegion;
        string symbol = region.CurrencySymbol;
        return symbol;
    }
}