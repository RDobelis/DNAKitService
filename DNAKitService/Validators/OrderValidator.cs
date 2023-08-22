using DNAKitService.Models;
using DNAKitService.Validators.Interfaces;

namespace DNAKitService.Validators
{
    public class OrderValidator : IOrderValidator
    {
        public bool IsValid(Order order)
        {
            if (order == null)
                return false;
            
            if (order.DeliveryDate <= DateTime.Today)
                return false;
            
            if (order.Quantity <= 0 || order.Quantity > 999)
                return false;
            
            return true;
        }
    }
}
