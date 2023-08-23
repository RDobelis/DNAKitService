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
                throw new NullOrderException($"{nameof(order)} data is null.");

            if (order.CustomerId <= 0)
                throw new InvalidOrderException($"{nameof(order.CustomerId)} is invalid.");
            
            if (order.DeliveryDate <= DateTime.Today)
                throw new InvalidOrderException($"{nameof(order.DeliveryDate)} is invalid.");
            
            if (order.Quantity <= 0 || order.Quantity > 999)
                throw new InvalidOrderException($"{nameof(order.Quantity)} is invalid.");
            
            return true;
        }
    }
}
