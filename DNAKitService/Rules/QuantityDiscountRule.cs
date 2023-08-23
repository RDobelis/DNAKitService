using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Rules.Interfaces;
using DNAKitService.Validators.Interfaces;

namespace DNAKitService.Rules
{
    public class QuantityDiscountRule : IDiscountRule
    {
        private readonly IOrderValidator _orderValidator;

        public QuantityDiscountRule(IOrderValidator orderValidator)
        {
            _orderValidator = orderValidator;
        }

        public bool IsApplicable(Order order)
        {
            if (!_orderValidator.IsValid(order))
                throw new InvalidOrderException("Order data is invalid.");

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
