using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HW2
{
    class OrderClass
    {
        private int cardNo, amount, price;
        private string senderID;

        public OrderClass(string s, int c, int a, int p) // Constructor
        {
            senderID = s;
            cardNo = c;
            amount = a;
            price = p;
        }

        // Get and set methods below.
        public string getSenderID()
        {
            return senderID;
        }
        public int getcardNo()
        {
            return cardNo;
        }
        public int getAmount()
        {
            return amount;
        }
        public int getPrice()
        {
            return price;
        }
        public void setSenderID(string str) { senderID = str; }
        public void setcardNo(int i) { cardNo = i; }
        public void setAmount(int i) { amount = i; }
        public void setPrice(int i) { price = i; }
    }
}
