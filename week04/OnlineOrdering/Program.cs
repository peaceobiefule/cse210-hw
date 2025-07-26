using System;

class Program
{
    static void Main(string[] args)
    {
        // Order 1
        Address address1 = new Address("123 Main St", "New York", "NY", "USA");
        Customer customer1 = new Customer("Alice Smith", address1);
        Order order1 = new Order(customer1);
        order1.AddProduct(new Product("Laptop", "LAP123", 1200, 1));
        order1.AddProduct(new Product("Mouse", "MOU456", 25, 2));

        DisplayOrder(order1);

        // Order 2
        Address address2 = new Address("456 King Rd", "Toronto", "ON", "Canada");
        Customer customer2 = new Customer("Bob Johnson", address2);
        Order order2 = new Order(customer2);
        order2.AddProduct(new Product("Monitor", "MON789", 200, 1));
        order2.AddProduct(new Product("Keyboard", "KEY987", 50, 1));

        DisplayOrder(order2);
    }

    static void DisplayOrder(Order order)
    {
        Console.WriteLine(order.GetShippingLabel());
        Console.WriteLine(order.GetPackingLabel());
        Console.WriteLine($"Total Cost: ${order.GetTotalCost():0.00}\n");
        Console.WriteLine(new string('-', 40));
    }
}
