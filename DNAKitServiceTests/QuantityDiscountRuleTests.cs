using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Rules;
using FluentAssertions;

namespace DNAKitService.Tests
{
    [TestFixture]
    public class QuantityDiscountRuleTests
    {
        private QuantityDiscountRule _rule;

        [SetUp]
        public void Setup()
        {
            _rule = new QuantityDiscountRule();
        }

        [TestCase(9, false)]
        [TestCase(10, true)]
        [TestCase(25, true)]
        public void IsApplicable_VariousQuantities_ReturnsExpectedResult(int quantity, bool expectedResult)
        {
            // Arrange & Act
            var result = CheckIsApplicableForQuantity(quantity);

            // Assert
            result.Should().Be(expectedResult);
        }

        [TestCase(10, 0.05)]
        [TestCase(50, 0.15)]
        public void CalculateDiscount_VariousQuantities_ReturnsExpectedDiscount(int quantity, double discountPercentage)
        {
            // Arrange & Act
            var discount = CalculateDiscountForQuantity(quantity);
            var expectedDiscount = new Order { Quantity = quantity, Kit = new BasicDnaKit() }.BasePrice * discountPercentage;

            // Assert
            discount.Should().Be(expectedDiscount);
        }

        [Test]
        public void IsApplicable_NullOrder_ThrowsInvalidOrderException()
        {
            // Arrange
            Order order = null;

            // Act
            Action action = () => _rule.IsApplicable(order);

            // Assert
            action.Should().Throw<InvalidOrderException>().WithMessage("Order cannot be null.");
        }

        [Test]
        public void IsApplicable_NegativeOrder_ThrowsInvalidOrderException()
        {
            // Arrange
            var order = new Order { Quantity = -1 };

            // Act
            Action action = () => _rule.IsApplicable(order);

            // Assert
            action.Should().Throw<InvalidOrderException>().WithMessage("Order quantity cannot be negative.");
        }

        [Test]
        public void CalculateDiscount_DiscountNotApplicable_ThrowsDiscountNotApplicableException()
        {
            // Arrange & Act
            Action action = () => CalculateDiscountForQuantity(5);

            // Assert
            action.Should().Throw<DiscountNotApplicableException>()
                .WithMessage("Discount is not applicable for this order.");
        }

        private bool CheckIsApplicableForQuantity(int quantity)
        {
            var order = new Order { Quantity = quantity };
            return _rule.IsApplicable(order);
        }

        private double CalculateDiscountForQuantity(int quantity)
        {
            var order = new Order
            {
                Quantity = quantity,
                Kit = new BasicDnaKit()
            };
            return _rule.CalculateDiscount(order);
        }
    }
}
