using DNAKitService.Models;

namespace DNAKitService.Validators.Interfaces
{
    public interface IOrderValidator
    {
        public bool IsValid(Order order);
    }
}
