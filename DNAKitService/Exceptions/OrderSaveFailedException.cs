namespace DNAKitService.Exceptions
{
    public class OrderSaveFailedException : Exception
    {
        public OrderSaveFailedException(string message) : base(message)
        {
        }
    }
}
