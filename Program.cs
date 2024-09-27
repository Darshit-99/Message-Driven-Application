using ProducerConsumerApp;
using System.Collections.Concurrent;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var orderQueue = new BlockingCollection<Order>();
        string logFilePath = "OrderProcessingLog.txt"; 

        Console.Write("Enter the number of orders you want to place: ");
        int numberOfOrders = int.Parse(Console.ReadLine());

        
        var producer = new OrderProducer(orderQueue);

        
        var consumer = new OrderConsumer(orderQueue, logFilePath);

        
        var producerTask = producer.ProduceOrdersAsync(numberOfOrders);
        var consumerTask = consumer.ProcessOrdersAsync();

        await Task.WhenAll(producerTask, consumerTask);

        
        Console.WriteLine($"Total processed orders: {consumer.GetSuccessCount()}");
        Console.WriteLine($"Total failed orders: {consumer.GetFailureCount()}");

        
        Console.WriteLine($"Logs written to {logFilePath}");
    }
}
