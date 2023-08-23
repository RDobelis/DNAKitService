using DNAKitService.Models;
using DNAKitService.Rules;
using DNAKitService.Rules.Interfaces;
using DNAKitService.Validators.Interfaces;
using FluentAssertions;
using Moq;

namespace DNAKitService.Tests
{
    [TestFixture]
    public class QuantityDiscountRuleTests
    {
        private IDiscountRule _rule;
        private Mock<IOrderValidator> _orderValidator;

        [SetUp]
        public void Setup()
        {
            _orderValidator = new Mock<IOrderValidator>();
            _rule = new QuantityDiscountRule(_orderValidator.Object);
        }

        [TestCase(9, false)]
        [TestCase(10, true)]
        [TestCase(25, true)]
        public void IsApplicable_VariousQuantities_ReturnsExpectedResult(int quantity, bool expectedResult)
        {
            // Arrange
            ValidateTrue();

            // Act
            var result = CheckIsApplicableForQuantity(quantity);

            // Assert
            result.Should().Be(expectedResult);
        }

        [TestCase(10, 0.05)]
        [TestCase(50, 0.15)]
        public void CalculateDiscount_VariousQuantities_ReturnsExpectedDiscount(int quantity, double discountPercentage)
        {
            // Arrange
            ValidateTrue();

            // Act
            var discount = CalculateDiscountForQuantity(quantity);
            var expectedDiscount = new Order { Quantity = quantity, Kit = new BasicDnaKit() }.BasePrice * discountPercentage;

            // Assert
            discount.Should().Be(expectedDiscount);
        }

        [Test]
        public void CalculateDiscount_DiscountNotApplicable_ReturnsZero()
        {
            // Arrange
            ValidateTrue();

            // Act
            var discount = CalculateDiscountForQuantity(5);

            // Assert
            discount.Should().Be(0);
        }

        private bool CheckIsApplicableForQuantity(int quantity)
        {
            var order = CreateOrder(quantity);
            return _rule.IsApplicable(order);
        }

        private double CalculateDiscountForQuantity(int quantity)
        {
            var order = CreateOrder(quantity);
            return _rule.CalculateDiscount(order);
        }

        private Order CreateOrder(int quantity)
        {
            return new Order
            {
                CustomerId = 1,
                Quantity = quantity,
                DeliveryDate = DateTime.Today.AddDays(10),
                Kit = new BasicDnaKit()
            };
        }

        private void ValidateTrue()
        {
            _orderValidator.Setup(validator => validator.IsValid(It.IsAny<Order>())).Returns(true);
        }
    }
}
