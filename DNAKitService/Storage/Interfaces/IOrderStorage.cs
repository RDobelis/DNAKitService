using DNAKitService.Models;

namespace DNAKitService.Storage.Interfaces
{
    public interface IOrderStorage
    {
        void SaveOrder(Order order);
        List<Order> GetOrders(int customerId);
    }
}
