using AboTracker.Model;
using System.Collections.Generic; // Added for IEnumerable

namespace AboTracker.Logic;

public static class Calculator
{
    
    public static double CalculateMonthlySum(IEnumerable<Subscription> subscriptions)
    {
        double totalYearly = 0;

        foreach (var s in subscriptions)
        {
            switch (s.PaymentPeriod)
            {
                case "Daily":
                    totalYearly += (double)(s.Amount * 365);
                    break;
                case "Weekly":
                    totalYearly += (double)(s.Amount * 52);
                    break;
                case "Quarterly":
                    totalYearly += (double)(s.Amount * 4);
                    break;
                case "Monthly": 
                    totalYearly += (double)(s.Amount * 12);
                    break;
                case "Yearly": 
                    totalYearly += (double)s.Amount;
                    break;
                default: 
                    // Unrecognized periods are ignored
                    break;
            }
        }
        
        return totalYearly / 12.0;
    }
}