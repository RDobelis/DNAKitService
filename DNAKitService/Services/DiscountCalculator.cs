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
            _orderValidator.IsValid(order);

            double totalDiscount = _discountRules
                .Where(rule => rule.IsApplicable(order))
                .Sum(rule => rule.CalculateDiscount(order));

            return totalDiscount;
        }
    }
}