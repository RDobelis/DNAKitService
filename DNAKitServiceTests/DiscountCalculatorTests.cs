using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Rules;
using DNAKitService.Rules.Interfaces;
using DNAKitService.Services;
using DNAKitService.Services.Interfaces;
using DNAKitService.Validators;
using DNAKitService.Validators.Interfaces;
using FluentAssertions;

namespace DNAKitService.Tests
{
    [TestFixture]
    public class DiscountCalculatorTests
    {
        private IDiscountCalculator _discountCalculator;
        private IOrderValidator _orderValidator;

        [SetUp]
        public void Setup()
        {
            _orderValidator = new OrderValidator();
            var discountRules = new List<IDiscountRule>
            {
                new QuantityDiscountRule(_orderValidator)
            };
            _discountCalculator = new DiscountCalculator(discountRules, _orderValidator);
        }

        [TestCase(10, 0.05)]
        [TestCase(50, 0.15)]
        public void CalculateDiscount_VariousQuantities_GivesExpectedDiscount(int quantity, double discountPercentage)
        {
            // Arrange
            var order = CreateOrder(quantity, new BasicDnaKit());
            var expectedDiscount = order.BasePrice * discountPercentage;

            // Act
            var discount = _discountCalculator.CalculateDiscount(order);

            // Assert
            discount.Should().Be(expectedDiscount);
        }

        [Test]
        public void CalculateDiscount_NullOrder_ThrowsNullDiscountOrderException()
        {
            // Arrange
            Order order = null;

            // Act
            Action act = () => _discountCalculator.CalculateDiscount(order);

            // Assert
            act.Should().Throw<InvalidOrderException>()
                .WithMessage("Order data is invalid.");
        }

        [Test]
        public void CalculateDiscount_NoApplicableDiscount_ThrowsNoDiscountApplicableException()
        {
            // Arrange
            var order = CreateOrder(5, new BasicDnaKit());

            // Act
            Action act = () => _discountCalculator.CalculateDiscount(order);

            // Assert
            act.Should().Throw<DiscountNotApplicableException>()
                .WithMessage("No discounts are applicable for the provided order.");
        }

        private Order CreateOrder(int quantity, BasicDnaKit kit)
        {
            return new Order
            {
                CustomerId = 1,
                Quantity = quantity,
                DeliveryDate = DateTime.Today.AddDays(10),
                Kit = kit
            };
        }
    }
}
