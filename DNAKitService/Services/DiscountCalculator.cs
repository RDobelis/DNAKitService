using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Rules.Interfaces;
using DNAKitService.Services.Interfaces;

namespace DNAKitService.Services
{
    public class DiscountCalculator : IDiscountCalculator
    {
        private readonly List<IDiscountRule> _discountRules;

        public DiscountCalculator(List<IDiscountRule> discountRules)
        {
            _discountRules = discountRules;
        }

        public double CalculateDiscount(Order order)
        {
            if (order == null)
                throw new NullOrderException("Cannot calculate discount for a null order.");

            double totalDiscount = _discountRules
                .Where(rule => rule.IsApplicable(order))
                .Sum(rule => rule.CalculateDiscount(order));

            if (totalDiscount == 0)
                throw new DiscountNotApplicableException("No discounts are applicable for the provided order.");

            return totalDiscount;
        }
    }
}