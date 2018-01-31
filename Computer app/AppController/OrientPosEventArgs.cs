using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppController
{
    public class OrientPosEventArgs : EventArgs
    {
        public OrientPosEventArgs(int pitch, int roll)
        {
            Pitch = pitch;
            Roll = roll;
        }

        public int Pitch { get; private set; }
        public int Roll { get; private set; }
    }
}
