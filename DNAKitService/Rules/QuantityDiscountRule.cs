using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Rules.Interfaces;

namespace DNAKitService.Rules
{
    public class QuantityDiscountRule : IDiscountRule
    {
        public bool IsApplicable(Order order)
        {
            if (order == null)
                throw new InvalidOrderException("Order cannot be null.");

            if (order.Quantity < 0)
                throw new InvalidOrderException("Order quantity cannot be negative.");

            return order.Quantity >= 10;
        }

        public double CalculateDiscount(Order order)
        {
            var basicDiscount = 0.05;
            var premiumDiscount = 0.15;

            if (order.Quantity >= 50)
                return order.BasePrice * premiumDiscount;

            if (order.Quantity >= 10)
                return order.BasePrice * basicDiscount;

            throw new DiscountNotApplicableException("Discount is not applicable for this order.");
        }
    }
}
