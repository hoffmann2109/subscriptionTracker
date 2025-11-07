using AboTracker.Logic;
using AboTracker.Model;
using Xunit;

namespace AboTracker.Tests
{
    public class CalculateUtilityTests
    {
        private readonly List<Subscription> _aboList = [];

        public CalculateUtilityTests()
        {
            var sub1 = new Subscription{
                Name = "Test 1", 
                Amount = 1, 
                PaymentPeriod = "Daily", 
                PurchaseDate = "28.10.2025", 
                NextPaymentDate = "29.10.2025", 
                Category = "Entertainment"
            };
            
            var sub2 = new Subscription{
                Name = "Test 2", 
                Amount = 3, 
                PaymentPeriod = "Weekly", 
                PurchaseDate = "10.10.2025", 
                NextPaymentDate = "17.10.2025", 
                Category = "Entertainment"
            };
            
            var sub3 = new Subscription{
                Name = "Test 3", 
                Amount = 10, 
                PaymentPeriod = "Monthly", 
                PurchaseDate = "28.10.2025", 
                NextPaymentDate = "28.11.2025", 
                Category = "Entertainment"
            };
            
            var sub4 = new Subscription{
                Name = "Test 4", 
                Amount = 40, 
                PaymentPeriod = "Quarterly", 
                PurchaseDate = "28.01.2025", 
                NextPaymentDate = "28.04.2025", 
                Category = "Entertainment"
            };
            
            var sub5 = new Subscription{
                Name = "Test 5", 
                Amount = 120, 
                PaymentPeriod = "Yearly", 
                PurchaseDate = "28.01.2025", 
                NextPaymentDate = "28.04.2025", 
                Category = "Entertainment"
            };
            
            _aboList.Add(sub1);
            _aboList.Add(sub2);
            _aboList.Add(sub3);
            _aboList.Add(sub4);
            _aboList.Add(sub5);
            
        }

        [Fact]
        public void CalculateMonthlySum_ComputesCorrectSum()
        {
            Assert.Equal(76.75,CalculateUtility.CalculateTotalMonthlySum(_aboList));
        }
        
    }
}
