using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Storage.Interfaces;

namespace DNAKitService.Storage
{
    public class OrderStorage : IOrderStorage
    {
        private readonly List<Order> _orders = new List<Order>();

        public void SaveOrder(Order order)
        {
            if (order == null)
                throw new NullOrderException("Cannot save a null order.");

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
