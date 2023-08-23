using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Services;
using DNAKitService.Services.Interfaces;
using DNAKitService.Storage.Interfaces;
using DNAKitService.Validators.Interfaces;
using FluentAssertions;
using Moq;

namespace DNAKitService.Tests
{
    [TestFixture]
    public class OrderManagerTests
    {
        private OrderManager _orderManager;
        private Mock<IOrderStorage> _orderStorage;
        private Mock<IDiscountCalculator> _discountCalculator;
        private Mock<IOrderValidator> _orderValidator;
        private static int CustomerId = 1;
        private static int Quantity = 1;
        private static DateTime DeliveryDate = DateTime.Today.AddDays(10);

        [SetUp]
        public void Setup()
        {
            _orderStorage = new Mock<IOrderStorage>();
            _discountCalculator = new Mock<IDiscountCalculator>();
            _orderValidator = new Mock<IOrderValidator>();

            _orderManager = new OrderManager(_orderStorage.Object, _discountCalculator.Object, _orderValidator.Object);
        }

        [Test]
        public void PlaceOrder_ValidOrder_OrderIsSaved()
        {
            // Arrange
            var order = CreateOrder(CustomerId, DeliveryDate, Quantity);
            _orderValidator.Setup(validator => validator.IsValid(order)).Returns(true);

            // Act
            var result = _orderManager.PlaceOrder(order);

            // Assert
            result.Should().BeTrue();
            _orderStorage.Verify(storage => storage.SaveOrder(order), Times.Once);
        }

        [Test]
        public void PlaceOrder_InvalidOrder_OrderIsNotSaved()
        {
            // Arrange
            var order = CreateOrder(CustomerId, DateTime.Today, Quantity);
            _orderValidator.Setup(validator => validator.IsValid(order)).Returns(false);

            // Act
            Action act = () => _orderManager.PlaceOrder(order);

            // Assert
            act.Should().Throw<InvalidOrderException>()
                .WithMessage("Order data is invalid.");
            _orderStorage.Verify(x => x.SaveOrder(order), Times.Never);
        }

        [Test]
        public void PlaceOrder_NullOrder_ThrowsInvalidOrderException()
        {
            // Arrange
            Order order = null;

            // Act
            Action act = () => _orderManager.PlaceOrder(order);

            // Assert
            act.Should().Throw<InvalidOrderException>()
                .WithMessage("Order data is invalid.");
        }

        [Test]
        public void PlaceOrder_SaveOrderFails_ThrowsOrderSaveFailedException()
        {
            // Arrange
            var order = CreateOrder(CustomerId, DeliveryDate, Quantity);
            _orderValidator.Setup(validator => validator.IsValid(order)).Returns(true);
            _orderStorage.Setup(storage => storage.SaveOrder(order))
                .Throws(new Exception("Database error"));

            // Act
            Action act = () => _orderManager.PlaceOrder(order);

            // Assert
            act.Should().Throw<OrderSaveFailedException>()
                .WithMessage("Failed to save the order. Reason: Database error");
        }

        [TestCase(5, 0.00)] 
        [TestCase(15, 0.05)] 
        [TestCase(60, 0.15)] 
        public void PlaceOrder_ApplyingDiscounts_FinalPriceIsCorrect(int orderQuantity, double expectedDiscountRate)
        {
            // Arrange
            var order = CreateOrder(CustomerId, DeliveryDate, orderQuantity);
            _orderValidator.Setup(validator => validator.IsValid(order)).Returns(true);
            var expectedDiscount = order.BasePrice * expectedDiscountRate * orderQuantity;
            _discountCalculator.Setup(calculator => calculator.CalculateDiscount(order)).Returns(expectedDiscount);

            // Act
            _orderManager.PlaceOrder(order);

            // Assert
            var expectedFinalPrice = (order.BasePrice * order.Quantity) - expectedDiscount;
            order.FinalPrice.Should().Be(expectedFinalPrice);
        }

        [Test]
        public void ListOrders_CustomerId_ReturnsOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                CreateOrder(CustomerId, DateTime.Today.AddDays(10), 1),
                CreateOrder(CustomerId, DateTime.Today.AddDays(15), 10),
                CreateOrder(CustomerId, DateTime.Today.AddDays(20), 100)
            };
            _orderStorage.Setup(storage => storage.GetOrders(CustomerId)).Returns(orders);

            // Act
            var result = _orderManager.ListOrders(CustomerId);

            // Assert
            result.Should().BeEquivalentTo(orders);
        }

        [Test]
        public void ListOrders_NoOrdersForCustomer_ThrowsOrdersNotFoundException()
        {
            // Arrange
            _orderStorage.Setup(storage => storage.GetOrders(CustomerId)).Returns(new List<Order>()); // No orders

            // Act
            Action act = () => _orderManager.ListOrders(CustomerId);

            // Assert
            act.Should().Throw<OrdersNotFoundException>()
                .WithMessage($"No orders found for CustomerId {CustomerId}.");
        }

        private Order CreateOrder(int customerId, DateTime deliveryDate, int quantity)
        {
            return new Order
            {
                CustomerId = customerId,
                DeliveryDate = deliveryDate,
                Quantity = quantity,
                Kit = new BasicDnaKit()
            };
        }
    }
}
