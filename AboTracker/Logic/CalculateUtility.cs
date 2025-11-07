using AboTracker.Model;

namespace AboTracker.Logic;

public static class CalculateUtility
{
    
    public static double CalculateTotalMonthlySum(IEnumerable<Subscription> subscriptions)
    {
        double totalYearly = 0;

        foreach (var s in subscriptions)
        {
            totalYearly += CalculateMonthlySum(s);
        }
        
        return totalYearly;
    }

    public static double CalculateMonthlySum(Subscription s)
    {
        double result = 0;
        
        switch (s.PaymentPeriod)
        {
            case "Daily":
                result = (double)(s.Amount * 365);
                break;
            case "Weekly":
                result = (double)(s.Amount * 52);
                break;
            case "Quarterly":
                result = (double)(s.Amount * 4);
                break;
            case "Monthly": 
                result = (double)(s.Amount * 12);
                break;
            case "Yearly": 
                result = (double)s.Amount;
                break;
        }

        return double.Round(result / 12, 2);
    }

    public static void ComputeNextPaymentDate(IEnumerable<Subscription> subscriptions)
    {
        var dateToday = DateTime.Now.Date; 
       
        foreach (var sub in subscriptions)
        {
            if (!DateTime.TryParse(sub.PurchaseDate, out var dateNextPayment))
            {
                sub.NextPaymentDate = "Invalid Date";
                continue;
            }
           
            dateNextPayment = dateNextPayment.Date;
            
            while (dateNextPayment < dateToday)
            {
                switch (sub.PaymentPeriod)
                {
                    case "Daily":
                        dateNextPayment = dateNextPayment.AddDays(1);
                        break;
                    case "Weekly":
                        dateNextPayment = dateNextPayment.AddDays(7);
                        break;
                    case "Monthly":
                        dateNextPayment = dateNextPayment.AddMonths(1);
                        break;
                    case "Quarterly":
                        dateNextPayment = dateNextPayment.AddMonths(3);
                        break;
                    case "Yearly":
                        dateNextPayment = dateNextPayment.AddYears(1);
                        break;
                    default:
                        dateNextPayment = dateToday.AddYears(100);
                        break;
                }
            }
            sub.NextPaymentDate = dateNextPayment > dateToday.AddYears(90)?"N/A":dateNextPayment.ToString("dd.MM.yyyy");
        }
    }

    public static IEnumerable<Subscription> SortList(IEnumerable<Subscription> listToSort, string? activeSort)
    {
        IEnumerable<Subscription> sortedList;

        var toSort = listToSort as Subscription[] ?? listToSort.ToArray();
        try
        {
            sortedList = activeSort switch
            {
                "Name (A-Z)" => toSort.OrderBy(s => s.Name),
                "Amount (High-Low)" => toSort.OrderByDescending(s => (s.Amount * GetMultiplier(s))),
                "Amount (Low-High)" => toSort.OrderBy(s => (s.Amount* GetMultiplier(s))),
                "Next Payment (Soonest)" => toSort.OrderBy(s => 
                    DateTime.TryParse(s.NextPaymentDate, out var date) ? date : DateTime.MaxValue),
                "Purchase Date (Newest)" => toSort.OrderByDescending(s => 
                    DateTime.TryParse(s.PurchaseDate, out var date) ? date : DateTime.MinValue),
                "Payment Period" => toSort.OrderBy(s => s.PaymentPeriod),
                "Category (A-Z)" => toSort.OrderBy(s => s.Category),
                _ => toSort // Default case
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during sorting: {ex.Message}");
            sortedList = toSort;
        }

        return sortedList;
    }

    private static int GetMultiplier(Subscription sub)
    {
        var multiplier = 1;
        
        switch (sub.PaymentPeriod)
        {
            case "Daily":
                multiplier = 365;
                break;
            case "Weekly":
                multiplier = 52;

                break;
            case "Monthly":
                multiplier = 12;
                break;
            case "Quarterly":
                multiplier = 4;
                break;
            default:
                multiplier = 1;
                break;
        }

        return multiplier;
    }
}