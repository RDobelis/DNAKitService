﻿using DNAKitService.Exceptions;
using DNAKitService.Models;
using DNAKitService.Storage;
using DNAKitService.Storage.Interfaces;
using DNAKitService.Validators;
using DNAKitService.Validators.Interfaces;
using FluentAssertions;

namespace DNAKitService.Tests
{
    [TestFixture]
    public class OrderStorageTests
    {
        private IOrderStorage _orderStorage;
        private IOrderValidator _orderValidator;
        private static int CustomerId = 1;
        private static int Quantity = 1;
        private static DateTime DeliveryDate = DateTime.Today.AddDays(10);

        [SetUp]
        public void Setup()
        {
            _orderValidator = new OrderValidator();
            _orderStorage = new OrderStorage(_orderValidator);
        }

        [Test]
        public void SaveOrder_ValidOrder_OrderIsSaved()
        {
            // Arrange
            var order = CreateOrder(CustomerId, DeliveryDate, Quantity);

            // Act
            _orderStorage.SaveOrder(order);

            // Assert
            var savedOrder = _orderStorage.GetOrders(1).FirstOrDefault();

            savedOrder.Should().BeEquivalentTo(order);
        }

        [Test]
        public void GetOrders_MultipleOrdersForCustomer_ReturnsAllOrders()
        {
            // Arrange
            var order1 = CreateOrder(CustomerId, DeliveryDate, Quantity);
            var order2 = CreateOrder(CustomerId, DeliveryDate, 2);

            _orderStorage.SaveOrder(order1);
            _orderStorage.SaveOrder(order2);

            // Act
            var orders = _orderStorage.GetOrders(CustomerId);

            // Assert
            orders.Should().Contain(order1);
            orders.Should().Contain(order2);
            orders.Should().HaveCount(2);
        }

        [Test]
        public void SaveOrder_NullOrder_ThrowsNullOrderException()
        {
            // Arrange
            Order order = null;
            
            // Act
            Action act = () => _orderStorage.SaveOrder(order);

            // Assert
            act.Should().Throw<InvalidOrderException>().WithMessage("Order data is invalid.");
        }

        [Test]
        public void SaveOrder_DuplicateOrder_ThrowsDuplicateOrderException()
        {
            // Arrange
            var order = CreateOrder(CustomerId, DeliveryDate, Quantity);
            _orderStorage.SaveOrder(order);

            // Act
            Action act = () => _orderStorage.SaveOrder(order);

            // Assert
            act.Should().Throw<DuplicateOrderException>()
                .WithMessage($"Order with CustomerId {order.CustomerId} and Quantity {order.Quantity} already exists.");
        }

        [Test]
        public void GetOrders_NonexistentCustomerId_ThrowsOrderNotFoundException()
        {
            // Arrange
            var nonExistentCustomerId = 999;

            // Act
            Action act = () => _orderStorage.GetOrders(nonExistentCustomerId);

            // Assert
            act.Should().Throw<OrderNotFoundException>()
                .WithMessage($"No orders found for CustomerId {nonExistentCustomerId}.");
        }

        private Order CreateOrder(int customerId, DateTime deliveryDate, int quantity)
        {
            return new Order
            {
                CustomerId = customerId,
                DeliveryDate = deliveryDate,
                Quantity = quantity
            };
        }
    }
}
