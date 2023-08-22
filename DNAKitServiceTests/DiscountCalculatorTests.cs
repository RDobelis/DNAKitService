using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Rules;
using DNAKitService.Rules.Interfaces;
using DNAKitService.Services;
using DNAKitService.Services.Interfaces;
using FluentAssertions;

namespace DNAKitService.Tests
{
    [TestFixture]
    public class DiscountCalculatorTests
    {
        private IDiscountCalculator _discountCalculator;

        [SetUp]
        public void Setup()
        {
            var discountRules = new List<IDiscountRule>
            {
                new QuantityDiscountRule()
            };
            _discountCalculator = new DiscountCalculator(discountRules);
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
            act.Should().Throw<NullOrderException>()
                .WithMessage("Cannot calculate discount for a null order.");
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
                Quantity = quantity,
                Kit = kit
            };
        }
    }
}
