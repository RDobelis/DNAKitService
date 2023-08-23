using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Services.Interfaces;
using DNAKitService.Storage.Interfaces;
using DNAKitService.Validators.Interfaces;

namespace DNAKitService.Services
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderStorage _orderStorage;
        private readonly IDiscountCalculator _discountCalculator;
        private readonly IOrderValidator _orderValidator;

        public OrderManager(IOrderStorage orderStorage, IDiscountCalculator discountCalculator,
            IOrderValidator orderValidator)
        {
            _orderStorage = orderStorage;
            _discountCalculator = discountCalculator;
            _orderValidator = orderValidator;
        }

        public bool PlaceOrder(Order order)
        {
            if (!_orderValidator.IsValid(order))
                throw new InvalidOrderException("Order data is invalid.");

            var discount = _discountCalculator.CalculateDiscount(order);
            order.FinalPrice = order.BasePrice * order.Quantity - discount;

            try
            {
                _orderStorage.SaveOrder(order);
            }
            catch (Exception ex)
            {
                throw new OrderSaveFailedException($"Failed to save the order. Reason: {ex.Message}");
            }

            return true;
        }

        public List<Order> ListOrders(int customerId)
        {
            var orders = _orderStorage.GetOrders(customerId);

            if (orders == null || orders.Count == 0)
                throw new OrdersNotFoundException($"No orders found for CustomerId {customerId}.");
            
            return orders;
        }
    }
}
