using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Rules;
using DNAKitService.Rules.Interfaces;
using DNAKitService.Services;
using DNAKitService.Services.Interfaces;
using DNAKitService.Validators.Interfaces;
using FluentAssertions;
using Moq;

namespace DNAKitService.Tests
{
    [TestFixture]
    public class DiscountCalculatorTests
    {
        private IDiscountCalculator _discountCalculator;
        private Mock<IOrderValidator> _orderValidator;

        [SetUp]
        public void Setup()
        {
            _orderValidator = new Mock<IOrderValidator>();
            _orderValidator.Setup(validator => validator.IsValid(It.IsAny<Order>())).Returns(true);
            var discountRules = new List<IDiscountRule>
            {
                new QuantityDiscountRule(_orderValidator.Object)
            };
            _discountCalculator = new DiscountCalculator(discountRules, _orderValidator.Object);
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
        public void CalculateDiscount_NoApplicableDiscount_ReturnsZero()
        {
            // Arrange
            var order = CreateOrder(5, new BasicDnaKit());

            // Act
            var discount = _discountCalculator.CalculateDiscount(order);

            // Assert
            discount.Should().Be(0);
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
