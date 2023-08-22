using DNAKitService.Models;

namespace DNAKitService.Rules.Interfaces
{
    public interface IDiscountRule
    {

        bool IsApplicable(Order order);
        double CalculateDiscount(Order order);
    }
}
