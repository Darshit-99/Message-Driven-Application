using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumerApp
{
    public class OrderConsumer
    {
        private readonly BlockingCollection<Order> _orderQueue;
        private int _successCount;    // Tracks the total number of successfully processed orders
        private int _failureCount;    // Tracks the total number of failed orders
        private readonly string _logFilePath;   // Path to log the processed results

        public OrderConsumer(BlockingCollection<Order> orderQueue, string logFilePath)
        {
            _orderQueue = orderQueue;
            _logFilePath = logFilePath;

            // Clear previous logs (optional)
            if (File.Exists(_logFilePath))
            {
                File.Delete(_logFilePath);
            }
        }

        public async Task ProcessOrdersAsync()
        {
            foreach (var order in _orderQueue.GetConsumingEnumerable())
            {
                try
                {
                    if (!order.IsInStock)
                    {
                        // Mark order as out of stock
                        order.OrderStatus = "Out of Stock";
                        _failureCount++;
                        Console.WriteLine($"Order {order.OrderId} failed. Item out of stock.");

                        // Log the out of stock case
                        Log($"ERROR: Order {order.OrderId} failed due to out of stock.");
                    }
                    else
                    {
                        await ProcessOrderAsync(order);
                        order.OrderStatus = "Processed";
                        _successCount++;
                        Console.WriteLine($"Order {order.OrderId} processed successfully.");

                        // Log success
                        Log($"SUCCESS: Order {order.OrderId} processed successfully.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle unexpected errors
                    _failureCount++;
                    Console.WriteLine($"Failed to process Order {order.OrderId}: {ex.Message}");
                    Log($"ERROR: Order {order.OrderId} failed. Reason: {ex.Message}");
                }

                // Simulate asynchronous processing delay
                await Task.Delay(200);
            }
        }

        private Task ProcessOrderAsync(Order order)
        {
            // Simulate successful order processing logic
            return Task.CompletedTask;
        }

        // Method to log success and error messages
        private void Log(string message)
        {
            using (StreamWriter writer = new StreamWriter(_logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }

        public int GetSuccessCount()
        {
            return _successCount;
        }

        public int GetFailureCount()
        {
            return _failureCount;
        }
    }
}
