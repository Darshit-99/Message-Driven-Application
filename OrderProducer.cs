using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ProducerConsumerApp
{
    public class OrderProducer
    {
        private readonly BlockingCollection<Order> _orderQueue;
        private readonly Random _random;

        public OrderProducer(BlockingCollection<Order> orderQueue)
        {
            _orderQueue = orderQueue;
            _random = new Random();
        }

        public async Task ProduceOrdersAsync(int numberOfOrders)
        {
            for (int i = 1; i <= numberOfOrders; i++)
            {
                var order = new Order
                {
                    OrderId = i,
                    CustomerName = $"Customer {i}",
                    OrderAmount = _random.Next(50, 500), 
                    IsInStock = _random.Next(0, 2) == 1  
                };

                _orderQueue.Add(order);
                Console.WriteLine($"Order placed: {order.OrderId}, Customer: {order.CustomerName}, Amount: {order.OrderAmount}, In Stock: {order.IsInStock}");

                
                await Task.Delay(100);
            }

            _orderQueue.CompleteAdding(); 
        }
    }
}
