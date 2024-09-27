using ProducerConsumerApp;
using System.Collections.Concurrent;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var orderQueue = new BlockingCollection<Order>();
        string logFilePath = "OrderProcessingLog.txt"; // File where logs will be written

        Console.Write("Enter the number of orders you want to place: ");
        int numberOfOrders = int.Parse(Console.ReadLine());

        // Create producer (user will input orders)
        var producer = new OrderProducer(orderQueue);

        // Create consumer (backend system processing orders)
        var consumer = new OrderConsumer(orderQueue, logFilePath);

        // Produce and process orders concurrently
        var producerTask = producer.ProduceOrdersAsync(numberOfOrders);
        var consumerTask = consumer.ProcessOrdersAsync();

        await Task.WhenAll(producerTask, consumerTask);

        // Log results
        Console.WriteLine($"Total processed orders: {consumer.GetSuccessCount()}");
        Console.WriteLine($"Total failed orders: {consumer.GetFailureCount()}");

        // Inform user about log location
        Console.WriteLine($"Logs written to {logFilePath}");
    }
}
