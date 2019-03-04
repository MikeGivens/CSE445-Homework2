using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
class MultiCellBuffer
{
    const int SIZE = 3;
    public int head = 0, tail = 0, elements = 0;
    string[] buffer = new string[SIZE];

    Semaphore readwrite = new Semaphore(3, 3); // Create the readwrite semaphore with count of 3.

    public void setOneCell(string str)
    {
        readwrite.WaitOne();
        lock (this) // Lock on the cell.
        {
            while (elements == SIZE) // If the buffer is full then wait to write.
            {
                Monitor.Wait(this);
            }

            buffer[tail] = str;
            tail = (tail + 1) % SIZE;
            elements++;
            readwrite.Release();
            Monitor.Pulse(this);
        }
    }

    public string getOneCell()
    {
        readwrite.WaitOne();
        lock (this) // Lock on the cell.
        {
            string element;
            while (elements == 0) // If the buffer is empty then wait to read.
            {
                Monitor.Wait(this);
            }

            element = buffer[head];
            head = (head + 1) % SIZE;
            elements--;
            readwrite.Release();
            Monitor.Pulse(this);
            return element;
        }
    }
}

