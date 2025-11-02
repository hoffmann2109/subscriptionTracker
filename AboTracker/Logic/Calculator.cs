using AboTracker.Model;

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
            }
        }
        
        return totalYearly / 12.0;
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
}