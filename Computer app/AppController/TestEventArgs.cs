using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppController
{
    public class TestEventArgs : EventArgs
    {
        public TestEventArgs(int data)
        {
            Data = data;
        }

        public int Data { get; private set; }
    }
}
