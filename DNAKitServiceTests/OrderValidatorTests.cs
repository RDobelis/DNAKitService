using DNAKitService.Models;
using DNAKitService.Validators;
using FluentAssertions;

namespace DNAKitService.Tests
{
    [TestFixture]
    public class OrderValidatorTests
    {
        private OrderValidator _orderValidator;

        [SetUp]
        public void Setup()
        {
            _orderValidator = new OrderValidator();
        }

        [TestCase(-1, "DeliveryDate in the future", ExpectedResult = false)]
        [TestCase(0, "DeliveryDate in the future", ExpectedResult = false)]
        [TestCase(1, "DeliveryDate in the future", ExpectedResult = true)]
        [TestCase(1000, "DeliveryDate in the future", ExpectedResult = false)]
        [TestCase(0, "Today's Date", ExpectedResult = false)]
        [TestCase(1, "Past Date", ExpectedResult = false)]
        public bool IsValid_VariousOrderScenarios_ReturnsExpectedValidity(int quantity, string dateScenario)
        {
            // Arrange
            var order = CreateOrder(quantity, dateScenario);

            // Act & Assert
            return _orderValidator.IsValid(order);
        }

        [Test]
        public void IsValid_NullOrder_ReturnsFalse()
        {
            // Arrange
            Order order = null;

            // Act
            var result = _orderValidator.IsValid(order);

            // Assert
            result.Should().BeFalse();
        }

        private Order CreateOrder(int quantity, string dateScenario)
        {
            DateTime deliveryDate;
            switch (dateScenario)
            {
                case "Past Date":
                    deliveryDate = DateTime.Today.AddDays(-1);
                    break;
                case "DeliveryDate in the future":
                    deliveryDate = DateTime.Today.AddDays(10);
                    break;
                default:
                    deliveryDate = DateTime.Today;
                    break;
            }

            return new Order
            {
                Quantity = quantity,
                DeliveryDate = deliveryDate
            };
        }
    }
}
