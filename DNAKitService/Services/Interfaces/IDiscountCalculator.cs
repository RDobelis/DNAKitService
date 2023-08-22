using DNAKitService.Models;

namespace DNAKitService.Services.Interfaces
{
    public interface IDiscountCalculator
    {
        double CalculateDiscount(Order order);
    }
}
