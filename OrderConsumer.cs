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
        private int _successCount;    
        private int _failureCount;    
        private readonly string _logFilePath;  

        public OrderConsumer(BlockingCollection<Order> orderQueue, string logFilePath)
        {
            _orderQueue = orderQueue;
            _logFilePath = logFilePath;

            
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
                        
                        order.OrderStatus = "Out of Stock";
                        _failureCount++;
                        Console.WriteLine($"Order {order.OrderId} failed. Item out of stock.");

                        
                        Log($"ERROR: Order {order.OrderId} failed due to out of stock.");
                    }
                    else
                    {
                        await ProcessOrderAsync(order);
                        order.OrderStatus = "Processed";
                        _successCount++;
                        Console.WriteLine($"Order {order.OrderId} processed successfully.");

                        
                        Log($"SUCCESS: Order {order.OrderId} processed successfully.");
                    }
                }
                catch (Exception ex)
                {
                    
                    _failureCount++;
                    Console.WriteLine($"Failed to process Order {order.OrderId}: {ex.Message}");
                    Log($"ERROR: Order {order.OrderId} failed. Reason: {ex.Message}");
                }

                
                await Task.Delay(200);
            }
        }

        private Task ProcessOrderAsync(Order order)
        {
            
            return Task.CompletedTask;
        }

        
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
