using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Tests.Helpers;
using DNAKitService.Validators;
using DNAKitService.Validators.Interfaces;
using FluentAssertions;

namespace DNAKitService.Tests
{
    [TestFixture]
    public class OrderValidatorTests
    {
        private IOrderValidator _orderValidator;

        [SetUp]
        public void Setup()
        {
            _orderValidator = new OrderValidator();
        }

        [TestCase(-1, DateScenario.DeliveryDateInTheFuture, false)]
        [TestCase(0, DateScenario.DeliveryDateInTheFuture, false)]
        [TestCase(1, DateScenario.DeliveryDateInTheFuture, true)]
        [TestCase(1000, DateScenario.DeliveryDateInTheFuture, false)]
        [TestCase(500, DateScenario.DeliveryDateInTheFuture, true)]
        public void IsValid_WithVariousQuantitiesAndFutureDeliveryDate_ShouldValidate(int quantity, DateScenario dateScenario,
            bool expectedValidity)
        {
            ValidateOrder(quantity, dateScenario, expectedValidity);
        }

        [TestCase(1, DateScenario.TodaysDate, false)]
        [TestCase(1000, DateScenario.TodaysDate, false)]
        public void IsValid_WithVariousQuantitiesAndTodaysDate_ShouldValidate(int quantity, DateScenario dateScenario,
            bool expectedValidity)
        {
            ValidateOrder(quantity, dateScenario, expectedValidity);
        }

        [TestCase(0, DateScenario.PastDate, false)]
        public void IsValid_WithVariousQuantitiesAndPastDate_ShouldValidate(int quantity, DateScenario dateScenario,
            bool expectedValidity)
        {
            ValidateOrder(quantity, dateScenario, expectedValidity);
        }

        [Test]
        public void IsValid_NullOrder_ThrowsException()
        {
            // Arrange
            Order order = null;

            // Act
            Action act = () => _orderValidator.IsValid(order);

            // Assert
            act.Should().Throw<NullOrderException>().WithMessage($"{nameof(order)} data is null.");
        }

        [Test]
        public void IsValid_InvalidCustomerId_ThrowsException()
        {
            // Arrange
            var order = CreateOrder(1, DateScenario.DeliveryDateInTheFuture);
            order.CustomerId = 0;

            // Act
            Action act = () => _orderValidator.IsValid(order);

            // Assert
            act.Should().Throw<InvalidOrderException>().WithMessage($"{nameof(order.CustomerId)} is invalid.");
        }

        private void ValidateOrder(int quantity, DateScenario dateScenario, bool expectedValidity)
        {
            var order = CreateOrder(quantity, dateScenario);

            if (expectedValidity)
            {
                _orderValidator.IsValid(order).Should().BeTrue();
            }
            else
            {
                Action act = () => _orderValidator.IsValid(order);
                act.Should().Throw<InvalidOrderException>();
            }
        }

        private Order CreateOrder(int quantity, DateScenario dateScenario)
        {
            DateTime deliveryDate = dateScenario switch
            {
                DateScenario.PastDate => DateTime.Today.AddDays(-1),
                DateScenario.DeliveryDateInTheFuture => DateTime.Today.AddDays(10),
                _ => DateTime.Today
            };

            return new Order
            {
                CustomerId = 1,
                Quantity = quantity,
                DeliveryDate = deliveryDate,
                Kit = new BasicDnaKit()
            };
        }
    }
}
