using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace HW2
{

    class TravelAgency
    {
        int amount;
        string encodedOrder = "";
        static int price = -1;
        static OrderClass order;
        static Random rnd = new Random();

        public void runAgency() // Thread start method.
        {
            HotelSupplier hs = new HotelSupplier();
            while(hs.getPrice() != -1)
            {
                Thread.Sleep(500);
                if (price != -1)
                    makeOrder();
            }
        }
        public void makeOrder() // Creates an order for encoding.
        {
            int newCard = rnd.Next(10000, 20001); // Creates a random card number.

            if (price > 140 || (price > 130 && Thread.CurrentThread.Name == "ta1")) // Chooses an amount to purchase.
                amount = 1;
            else if (price <= 140 && price > 115)
                amount = 2;
            else amount = 3;

            Console.WriteLine("{0} making order for {1} room(s) priced at ${2}", Thread.CurrentThread.Name, amount, price);
            order = new OrderClass(Thread.CurrentThread.Name, newCard, amount, price);
            encoder(order);

        }
        public void encoder(OrderClass orderIn) // Encodes an order.
        {
            lock (this) // Lock so that values don't get mixed between threads.
            {
                while (encodedOrder != "")
                {
                    Monitor.Wait(this);
                }
                string orderString = orderIn.getSenderID() + "," + orderIn.getcardNo().ToString() + "," + orderIn.getAmount().ToString() + "," + orderIn.getPrice(); // create string of values from OrderClass object.
                for (int i = 0; i < orderString.Length; i++)
                {
                    encodedOrder += (char)(orderString[i] + 1); // +1 to ASCII value encoder.
                }
                sendOrderEncoded(encodedOrder);
                encodedOrder = "";
            }
        }
        public void sendOrderEncoded(string str) // Puts order in the multicellbuffer.
        {
            Program.mcb.setOneCell(str);
        }
        public void roomOnSale(int p) // Event handler for a priceCutEvent.
        {
            Console.WriteLine("Hotel supplier, {0}, has a room on sale at: ${1}", Thread.CurrentThread.Name, p);
            price = p;
        }
    }
}
