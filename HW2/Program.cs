using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
// Name: Michael Givens
// Environment: Visual Studio 2015

public delegate void priceCutEvent(Int32 pr);

namespace HW2
{
    class Program
    {
        public static HotelSupplier hs1 = new HotelSupplier();
        public static HotelSupplier hs2 = new HotelSupplier();
        public static MultiCellBuffer mcb = new MultiCellBuffer();
        static void Main(string[] args)
        {

            Thread hsThread1 = new Thread(new ThreadStart(hs1.runSupplier)); // Create first supplier thread.
            hsThread1.Name = "hs1";
            hsThread1.Start();

            Thread hsThread2 = new Thread(new ThreadStart(hs2.runSupplier)); // Create second supplier thread.
            hsThread2.Name = "hs2";
            hsThread2.Start();

            TravelAgency ta = new TravelAgency();
            HotelSupplier.priceCut += new priceCutEvent(ta.roomOnSale);

            Thread[] agencies = new Thread[5];
            for (int i = 0; i < 5; i++) // Create 5 agency threads.
            {
                agencies[i] = new Thread(new ThreadStart(ta.runAgency));
                agencies[i].Name = "ta" + (i + 1).ToString();
                agencies[i].Start();
            }

            hsThread1.Join();
            hsThread2.Join();
        }
    }
}
