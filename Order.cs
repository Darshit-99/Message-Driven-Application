using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumerApp
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public double OrderAmount { get; set; }
        public bool IsInStock { get; set; }
        public string OrderStatus { get; set; }  // Tracks the order status (e.g., "Processed", "Out of Stock")
    }
}
