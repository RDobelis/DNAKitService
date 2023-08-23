using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Rules.Interfaces;
using DNAKitService.Services.Interfaces;
using DNAKitService.Validators.Interfaces;

namespace DNAKitService.Services
{
    public class DiscountCalculator : IDiscountCalculator
    {
        private readonly List<IDiscountRule> _discountRules;
        private readonly IOrderValidator _orderValidator;

        public DiscountCalculator(List<IDiscountRule> discountRules, IOrderValidator orderValidator)
        {
            _discountRules = discountRules;
            _orderValidator = orderValidator;
        }

        public double CalculateDiscount(Order order)
        {
            if (!_orderValidator.IsValid(order))
                throw new InvalidOrderException("Order data is invalid.");

            double totalDiscount = _discountRules
                .Where(rule => rule.IsApplicable(order))
                .Sum(rule => rule.CalculateDiscount(order));

            if (totalDiscount == 0)
                throw new DiscountNotApplicableException("No discounts are applicable for the provided order.");

            return totalDiscount;
        }
    }
}