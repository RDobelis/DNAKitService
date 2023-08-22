namespace DNAKitService.Exceptions
{
    public class OrdersNotFoundException : Exception
    {
        public OrdersNotFoundException(string message) : base(message)
        {
        }
    }
}
