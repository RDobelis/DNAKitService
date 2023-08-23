using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Validators.Interfaces;

namespace DNAKitService.Validators
{
    public class OrderValidator : IOrderValidator
    {
        public bool IsValid(Order order)
        {
            if (order == null)
                throw new NullOrderException("Order data is null.");

            if (order.CustomerId <= 0)
                throw new InvalidOrderException("CustomerId is invalid.");
            
            if (order.DeliveryDate <= DateTime.Today)
                throw new InvalidOrderException("Delivery date is invalid.");
            
            if (order.Quantity <= 0 || order.Quantity > 999)
                throw new InvalidOrderException("Quantity is invalid.");
            
            return true;
        }
    }
}
