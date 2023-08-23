using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Storage.Interfaces;
using DNAKitService.Validators.Interfaces;

namespace DNAKitService.Storage
{
    public class OrderStorage : IOrderStorage
    {
        private readonly List<Order> _orders = new List<Order>();
        private readonly IOrderValidator _orderValidator;

        public OrderStorage(IOrderValidator orderValidator)
        {
            _orderValidator = orderValidator;
        }

        public void SaveOrder(Order order)
        {
            if (!_orderValidator.IsValid(order))
                throw new InvalidOrderException("Order data is invalid.");

            if (_orders.Any(o => o.CustomerId == order.CustomerId && o.Quantity == order.Quantity))
                throw new DuplicateOrderException(
                    $"Order with CustomerId {order.CustomerId} and Quantity {order.Quantity} already exists.");

            _orders.Add(order);
        }

        public List<Order> GetOrders(int customerId)
        {
            var orders = _orders.Where(o => o.CustomerId == customerId).ToList();

            if (!orders.Any())
                throw new OrderNotFoundException($"No orders found for CustomerId {customerId}.");

            return orders;
        }
    }
}
