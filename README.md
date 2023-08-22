# DNA Kit Service

The DNA Kit Service is a simple library for handling order placement for DNA testing kits. It allows users to place orders, view orders, and apply discounts based on order quantity.

## Features

- Place orders for DNA testing kits
- List all orders for a customer
- Apply discounts based on order quantity:
  - 5% discount for orders of 10 or more kits
  - 15% discount for orders of 50 or more kits
- Validate orders:
  - Reject orders with delivery dates in the past
  - Reject orders with non-positive or non-round quantities
  - Reject orders with quantities greater than 999
- Designed for future extensibility with multiple kit variants and different base prices

## Technical Requirements

- Developed in C# programming language
- Utilizes Test-Driven Development (TDD) with NUnit and FluentAssertions
- Uses SOLID principles for clean and maintainable code

## Usage

1. Create an instance of the `OrderManager` class.
2. Use the `PlaceOrder` method to place orders for DNA testing kits.
3. Use the `ListOrders` method to view all orders for a customer.

## Contributing

Contributions are welcome! Please fork the repository and create a pull request with your changes. Make sure to include tests for any new features or bug fixes.
