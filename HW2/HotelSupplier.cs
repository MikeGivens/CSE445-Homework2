using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HW2
{
    class HotelSupplier
    {
        static char delimiterChar = ',';
        static Random rnd = new Random();
        public static event priceCutEvent priceCut;
        public int cuts = 0;
        public static int roomPrice = 125;
        public string encodedOrder, decodedStr;

        public HotelSupplier() // Constructor
        {
            encodedOrder = "";
            decodedStr = "";
        }
        public int getPrice() { return roomPrice; } // returns roomPrice.
        public void runSupplier() // Start method of thread.
        {
            while (cuts < 10) // End thread after 10 price cuts.
            {
                while (Program.mcb.elements != 0) // fetchs orders from the multicellbuffer before getting a new price.
                {
                    if (Program.mcb.elements != 0)
                        fetchOrder();
                }
                PricingModel(); // set new price.
            }
                
            if (cuts == 10)
                roomPrice = -1; // used to end agency threads.

            while (Program.mcb.elements != 0) // fetches any remaining orders.
            {
                if (Program.mcb.elements != 0)
                    fetchOrder();
            }
        }
        public void PricingModel() // Decides price of the room.
        {
            lock (this)
            {
                Thread.Sleep(500);
                int newPrice = rnd.Next(100, 151);
                if (newPrice < roomPrice & newPrice != -1)
                    if (priceCut != null)
                    {
                        priceCut(newPrice);
                        cuts++;
                    }
                roomPrice = newPrice;
            }

        }
        public void fetchOrder() // Gets an order from the multicellbuffer.
        {
            encodedOrder = Program.mcb.getOneCell();
            decoder(encodedOrder);
        }
        public void startProcessingThread(OrderClass oc) // Creates the order processing threads.
        {
            var processingThread = new Thread(() => OrderProcessing(oc));
            processingThread.Start();
        }
        public void OrderProcessing(OrderClass orderIn) // Checks credit card number and prints the final order detals.
        {
            bool cardConf = false;
            int preTax = 0;
            double totalCharge = 0;
            if (orderIn.getcardNo() > 10000 && orderIn.getcardNo() < 20001)
                cardConf = true;
            if (cardConf == true)
            {
                preTax = orderIn.getAmount() * orderIn.getPrice();
                totalCharge = preTax * 1.05; // 1.05 is a 5% tax.
                Console.WriteLine("Order from {0} CONFIRMED\nORDER INFO:\nRoom Price: ${2}\nAmount: {3}\nTax 5%\nTotal Cost: ${1}\n", orderIn.getSenderID(), totalCharge, orderIn.getPrice(), orderIn.getAmount());
            }
            else { Console.WriteLine("Error: invalid card"); }
        }
        public void decoder(string eo) // Decodes an order.
        {
            lock (this) // Lock so that values don't get mixed between threads.
            {
                while (decodedStr != "")
                {
                    Monitor.Wait(this);
                }
                for (int i = 0; i < eo.Length; i++)
                {
                    decodedStr += (char)(eo[i] - 1); // Decode the +1 ASCII encryption.
                }

                string[] words = decodedStr.Split(delimiterChar); // Parses the string into words delimited by a comma.
                string id = words[0];
                int cn = Convert.ToInt32(words[1]);
                int a = Convert.ToInt32(words[2]);
                int p = Convert.ToInt32(words[3]);

                OrderClass dOrder = new OrderClass(id, cn, a, p); // Create the order from words.
                startProcessingThread(dOrder); // Begin processing.
                decodedStr = "";
            }
        }
    }
}
