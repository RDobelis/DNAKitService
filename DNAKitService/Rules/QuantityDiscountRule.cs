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
            _orderValidator.IsValid(order);

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

            return 0;
        }
    }
}
