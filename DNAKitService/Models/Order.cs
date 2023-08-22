namespace DNAKitService.Models
{
    public class Order
    {
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public DateTime DeliveryDate { get; set; }
        public KitVariant Kit { get; set; }
        public double BasePrice => Kit.Price;
        public double FinalPrice { get; set; }
    }
}
