using DNAKitService.Models;

namespace DNAKitService.Services.Interfaces
{
    public interface IOrderManager
    {
        public bool PlaceOrder(Order order);
        public List<Order> ListOrders(int customerId);
    }
}
